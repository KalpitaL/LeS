﻿/*
 jQuery twitter bootstrap wizard plugin
 Examples and documentation at: http://github.com/VinceG/twitter-bootstrap-wizard
 version 1.4.2
 Requires jQuery v1.3.2 or later
 Supports Bootstrap 2.2.x, 2.3.x, 3.0
 Dual licensed under the MIT and GPL licenses:
 http://www.opensource.org/licenses/mit-license.php
 http://www.gnu.org/licenses/gpl.html
 Authors: Vadim Vincent Gabriel (http://vadimg.com), Jason Gill (www.gilluminate.com)
*/

(function (c) {
    var n = function (d, k) {
        d = c(d); var a = this, h = [],
            b = c.extend({}, c.fn.bootstrapWizard.defaults, k), f = null, e = null;
        this.rebindClick = function (b, a) { b.unbind("click", a).bind("click", a) };
        this.fixNavigationButtons = function () {
            f.length || (e.find("a:first").tab("show"), f = e.find('li:has([data-toggle="tab"]):first')); c(b.previousSelector, d).toggleClass("b", a.firstIndex() >= a.currentIndex()); c(b.nextSelector, d).toggleClass("disabled", a.currentIndex() >= a.navigationLength());
            c(b.nextSelector, d).toggleClass("hidden",a.currentIndex() >= a.navigationLength() && 0 < c(b.finishSelector, d).length);
            c(b.lastSelector, d).toggleClass("hidden", a.currentIndex() >= a.navigationLength() && 0 < c(b.finishSelector, d).length);
            c(b.finishSelector, d).toggleClass("hidden", a.currentIndex() < a.navigationLength());
            c(b.backSelector, d).toggleClass("disabled", 0 == h.length);
            c(b.backSelector, d).toggleClass("hidden", a.currentIndex() >= a.navigationLength() && 0 < c(b.finishSelector, d).length);
            a.rebindClick(c(b.nextSelector, d), a.next); a.rebindClick(c(b.previousSelector, d), a.previous);
            a.rebindClick(c(b.lastSelector, d), a.last); a.rebindClick(c(b.firstSelector, d), a.first);
            a.rebindClick(c(b.finishSelector, d), a.finish); a.rebindClick(c(b.backSelector, d), a.back);
            if (b.onTabShow && "function" === typeof b.onTabShow && !1 === b.onTabShow(f, e, a.currentIndex())) return !1
        }; this.next = function (g) {
            if (d.hasClass("last") || b.onNext && "function" === typeof b.onNext && !1 === b.onNext(f, e, a.nextIndex())) return !1;
            g = a.currentIndex(); var c = a.nextIndex();
            c > a.navigationLength() || (h.push(g), e.find('li:has([data-toggle="tab"])' + (b.withVisible ? ":visible" : "") + ":eq(" + c + ") a").tab("show"))
        }; this.previous = function (g) { if (d.hasClass("first") || b.onPrevious && "function" === typeof b.onPrevious && !1 === b.onPrevious(f, e, a.previousIndex())) return !1; g = a.currentIndex(); var c = a.previousIndex(); 0 > c || (h.push(g), e.find('li:has([data-toggle="tab"])' + (b.withVisible ? ":visible" : "") + ":eq(" + c + ") a").tab("show")) }; this.first = function (g) {
            if (b.onFirst && "function" === typeof b.onFirst && !1 === b.onFirst(f, e, a.firstIndex()) || d.hasClass("disabled")) return !1; h.push(a.currentIndex());
            e.find('li:has([data-toggle="tab"]):eq(0) a').tab("show")
        }; this.last = function (g) { if (b.onLast && "function" === typeof b.onLast && !1 === b.onLast(f, e, a.lastIndex()) || d.hasClass("disabled")) return !1; h.push(a.currentIndex()); e.find('li:has([data-toggle="tab"]):eq(' + a.navigationLength() + ") a").tab("show") }; this.finish = function (g) { if (b.onFinish && "function" === typeof b.onFinish) b.onFinish(f, e, a.lastIndex()) }; this.back = function () {
            if (0 == h.length) return null; var a = h.pop(); if (b.onBack && "function" === typeof b.onBack &&
            !1 === b.onBack(f, e, a)) return h.push(a), !1; d.find('li:has([data-toggle="tab"]):eq(' + a + ") a").tab("show")
        }; this.currentIndex = function () { return e.find('li:has([data-toggle="tab"])' + (b.withVisible ? ":visible" : "")).index(f) }; this.firstIndex = function () { return 0 }; this.lastIndex = function () { return a.navigationLength() }; this.getIndex = function (a) { return e.find('li:has([data-toggle="tab"])' + (b.withVisible ? ":visible" : "")).index(a) }; this.nextIndex = function () {
            var a = this.currentIndex(), c; do a++, c = e.find('li:has([data-toggle="tab"])' +
            (b.withVisible ? ":visible" : "") + ":eq(" + a + ")"); while (c && c.hasClass("disabled")); return a
        }; this.previousIndex = function () { var a = this.currentIndex(), c; do a--, c = e.find('li:has([data-toggle="tab"])' + (b.withVisible ? ":visible" : "") + ":eq(" + a + ")"); while (c && c.hasClass("disabled")); return a }; this.navigationLength = function () { return e.find('li:has([data-toggle="tab"])' + (b.withVisible ? ":visible" : "")).length - 1 }; this.activeTab = function () { return f }; this.nextTab = function () {
            return e.find('li:has([data-toggle="tab"]):eq(' +
            (a.currentIndex() + 1) + ")").length ? e.find('li:has([data-toggle="tab"]):eq(' + (a.currentIndex() + 1) + ")") : null
        }; this.previousTab = function () { return 0 >= a.currentIndex() ? null : e.find('li:has([data-toggle="tab"]):eq(' + parseInt(a.currentIndex() - 1) + ")") }; this.show = function (b) { b = isNaN(b) ? d.find('li:has([data-toggle="tab"]) a[href="#' + b + '"]') : d.find('li:has([data-toggle="tab"]):eq(' + b + ") a"); 0 < b.length && (h.push(a.currentIndex()), b.tab("show")) }; this.disable = function (a) {
            e.find('li:has([data-toggle="tab"]):eq(' + a +
            ")").addClass("disabled")
        }; this.enable = function (a) { e.find('li:has([data-toggle="tab"]):eq(' + a + ")").removeClass("disabled") }; this.hide = function (a) { e.find('li:has([data-toggle="tab"]):eq(' + a + ")").hide() }; this.display = function (a) { e.find('li:has([data-toggle="tab"]):eq(' + a + ")").show() };
        this.remove = function (a) { var b = "undefined" != typeof a[1] ? a[1] : !1; a = e.find('li:has([data-toggle="tab"]):eq(' + a[0] + ")"); b && (b = a.find("a").attr("href"), c(b).remove()); a.remove() }; var l = function (d) {
            var g = e.find('li:has([data-toggle="tab"])');
            d = g.index(c(d.currentTarget).parent('li:has([data-toggle="tab"])')); g = c(g[d]); if (b.onTabClick && "function" === typeof b.onTabClick && !1 === b.onTabClick(f, e, a.currentIndex(), d, g)) return !1
        }, m = function (d) { d = c(d.target).parent(); var g = e.find('li:has([data-toggle="tab"])').index(d); if (d.hasClass("disabled") || b.onTabChange && "function" === typeof b.onTabChange && !1 === b.onTabChange(f, e, a.currentIndex(), g)) return !1; f = d; a.fixNavigationButtons() }; this.resetWizard = function () {
            c('a[data-toggle="tab"]', e).off("click", l);
            c('a[data-toggle="tab"]', e).off("show show.bs.tab", m); e = d.find("ul:first", d); f = e.find('li:has([data-toggle="tab"]).active', d); c('a[data-toggle="tab"]', e).on("click", l); c('a[data-toggle="tab"]', e).on("show show.bs.tab", m); a.fixNavigationButtons()
        }; e = d.find("ul:first", d); f = e.find('li:has([data-toggle="tab"]).active', d); e.hasClass(b.tabClass) || e.addClass(b.tabClass); if (b.onInit && "function" === typeof b.onInit) b.onInit(f, e, 0); if (b.onShow && "function" === typeof b.onShow) b.onShow(f, e, a.nextIndex()); c('a[data-toggle="tab"]',
        e).on("click", l); c('a[data-toggle="tab"]', e).on("show show.bs.tab", m)
    }; c.fn.bootstrapWizard = function (d) { if ("string" == typeof d) { var k = Array.prototype.slice.call(arguments, 1); 1 === k.length && k.toString(); return this.data("bootstrapWizard")[d](k) } return this.each(function (a) { a = c(this); if (!a.data("bootstrapWizard")) { var h = new n(a, d); a.data("bootstrapWizard", h); h.fixNavigationButtons() } }) }; c.fn.bootstrapWizard.defaults = {
        withVisible: !0, tabClass: "nav nav-pills", nextSelector: ".wizard li.next", previousSelector: ".wizard li.previous",
        firstSelector: ".wizard li.first", lastSelector: ".wizard li.last", finishSelector: ".wizard li.finish", backSelector: ".wizard li.back", onShow: null, onInit: null, onNext: null, onPrevious: null, onLast: null, onFirst: null, onFinish: null, onBack: null, onTabChange: null, onTabClick: null, onTabShow: null
    }
})(jQuery);