��ࡱ�                !  ��	               -          ,      ����    :   ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������Return-Path: <mach-2+caf_=fuji_mach=lighthouse-esolutions.com@msdmail.fujitrading.co.jp>
Received: from pop4.netcore.co.in ([unix socket])	 by pop4.netcore.co.in with LMTPA;	
 Thu, 12 Dec 2019 17:39:02 +0530
Received: from newuucp1.netcore.co.in (uucp1.netcore.co.in [202.162.229.12])	by
 pop4.netcore.co.in (Postfix) with ESMTP id 85AA1601E14	for
 <fuji_mach@pop2.lighthouse-esolutions.com>; Thu, 12 Dec 2019 17:39:01 +0530
 (IST)
Received: from secure6.netcore.co.in (smtp246.mailserv.in [202.162.242.246])	by
 newuucp1.netcore.co.in (Postfix) with ESMTP id 4681A29FF6	for
 <fuji_mach@lighthouse-esolutions.com>; Thu, 12 Dec 2019 17:39:00 +0530 (IST)
Received: from newsmta1.netcore.co.in (smta1.netcore.co.in [202.162.242.51])	by
 secure6.netcore.co.in (Postfix) with ESMTP id 6F6859C15;	Thu, 12 Dec 2019
 17:38:40 +0530 (IST)
Received: from mail-wr1-f51.google.com (mail-wr1-f51.google.com [209.85.221.51])	by
 newsmta1.netcore.co.in (Postfix) with ESMTP id 292B12570009	for
 <fuji_mach@lighthouse-esolutions.com>; Thu, 12 Dec 2019 17:38:38 +0530 (IST)
Received: by mail-wr1-f51.google.com with SMTP id b6so2513443wrq.0        for
 <fuji_mach@lighthouse-esolutions.com>; Thu, 12 Dec 2019 04:08:38 -0800 (PST)
Received: by 2002:adf:afe6:0:0:0:0:0 with SMTP id y38csp530786wrd;        Thu, 12 Dec
 2019 04:08:35 -0800 (PST)
Received: from FUJIDOM2.fujitrading.co.jp (fujidom2.fujitrading.co.jp. [218.44.250.29])
        by mx.google.com with ESMTP id
 g23si4991187pfh.137.2019.12.12.04.08.35        for
 <mach-2@msdmail.fujitrading.co.jp>;        Thu, 12 Dec 2019 04:08:35 -0800
 (PST)
Received: from barracuda.fujitransport.co.jp ([192.168.200.10])          by
 FUJIDOM2.fujitrading.co.jp (IBM Domino Release 10.0.1FP2 HF10)          with
 ESMTP id 2019121221083467-421596 ;          Thu, 12 Dec 2019 21:08:34 +0900 
Received: from APC01-HK2-obe.outbound.protection.outlook.com
 (mail-eopbgr1300132.outbound.protection.outlook.com [40.107.130.132]) by
 barracuda.fujitransport.co.jp with ESMTP id 86WWxDquNDfJeHhG for
 <mach-2@fujitrading.co.jp>; Thu, 12 Dec 2019 21:08:33 +0900 (JST)
Received: from HK0PR01CA0061.apcprd01.prod.exchangelabs.com (2603:1096:203:a6::25) by
 HK2PR01MB3170.apcprd01.prod.exchangelabs.com (2603:1096:202:1b::12) with
 Microsoft SMTP Server (version=TLS1_2,
 cipher=TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384) id 15.20.2516.18; Thu, 12 Dec
 2019 12:08:31 +0000
Received: from PU1APC01FT023.eop-APC01.prod.protection.outlook.com
 (2a01:111:f400:7ebd::208) by HK0PR01CA0061.outlook.office365.com
 (2603:1096:203:a6::25) with Microsoft SMTP Server (version=TLS1_2,
 cipher=TLS_ECDHE_RSA_WITH_AES_256_GCM_SHA384) id 15.20.2538.16 via Frontend
 Transport; Thu, 12 Dec 2019 12:08:31 +0000
Received: from mail.executiveship.com (121.200.251.220) by
 PU1APC01FT023.mail.protection.outlook.com (10.152.253.4) with Microsoft SMTP
 Server (version=TLS1_2, cipher=TLS_ECDHE_RSA_WITH_AES_256_CBC_SHA384) id
 15.20.2538.15 via Frontend Transport; Thu, 12 Dec 2019 12:08:30 +0000
Received: from EsmitDVt005App (119.81.76.122) by ESMSGEXCH13.executiveship.com
 (2002:c8c8:119::c8c8:119) with Microsoft SMTP Server (TLS) id 15.0.1367.3;
 Thu, 12 Dec 2019 20:08:19 +0800
X-Sieve: CMU Sieve 2.4
X-Envelope-From: <mach-2+caf_=fuji_mach=lighthouse-esolutions.com@msdmail.fujitrading.co.jp>
X-Google-DKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed;        d=1e100.net; s=20161025;       
 h=x-gm-message-state:delivered-to:dkim-signature:mime-version:from:to       
  :cc:date:subject:message-id:auto-submitted:content-transfer-encoding;      
  bh=l5Q/VKp07PM2V2NeGdKq0Lm/DTxmfWXlHsY7BHC5grY=;       
 b=ZJlB5LNQ2MP9TNX3KYyW0MCr/sTYThTJUz98D3hi0SiQsK7s9ftsQafWCBfUXnzMR/        
 +vfVTWsDtyJam7OY0T21o4FGmoKLzUyOVzf1ZrTJHyuFvou+CJdD2TEeMH0IK7lKq1XW        
 Gi6nfoeMfiIW1ceYQZill0Yvl5WikLbxBhTlhU3euJujyqFHXqAT6h6lNKJx9Tw19+bn        
 9D+sIeUP2SFvwz7PAb67S8fXIMr0GYQGW6ejoLx/KUTfIvvobfbiCk4IN6pK2zlrOVDq        
 GpTZ2oHPfSWGMnuoUhTfQvGQaFL/N6Eip03i39IfL4Q4F/9xO+4vGv3eqsjeXm1RCS2w        
 FdvA==
X-Gm-Message-State: APjAAAXcXSJpat3zMhZcG64V1pVo/ikdBl9Ha6z/kyWz95GKK6c+FvyE	50WZdya1LzWnOTCA53SrnTihdOvHPaLWrenvPP1NJAXJq9QcE5c=
X-Received: by 2002:a5d:480b:: with SMTP id l11mr6040214wrq.129.1576152518541;       
 Thu, 12 Dec 2019 04:08:38 -0800 (PST)
X-Received: by 2002:a17:90a:1704:: with SMTP id z4mr9582960pjd.131.1576152515668;       
 Thu, 12 Dec 2019 04:08:35 -0800 (PST)
X-Forwarded-To: xdw-okumurak@fujitrading.co.jp, fuji_mach@lighthouse-esolutions.com
X-Forwarded-For: mach-2@msdmail.fujitrading.co.jp xdw-okumurak@fujitrading.co.jp,
 fuji_mach@lighthouse-esolutions.com
Delivered-To: mach-2@msdmail.fujitrading.co.jp
X-Google-Smtp-Source: APXvYqzcR8TrZ7ztuLtuHF+c3z9mZnXdkXr/M2FDbeJOV4S7RvipHooXmPKT4CRhzIpiVfA63xl1
ARC-Seal: i=2; a=rsa-sha256; t=1576152515; cv=pass;        d=google.com;
 s=arc-20160816;       
 b=TQaJHAENe3hTj5gDT0tsgRt8AJko/qRaLNsJJjL+eQOIYKo6njYa/lOzd0p/hf305e        
 vQoB7PCRiaz1vGfaO3bg/tGbNmouflgvKfX7Py+IzCGBa3kQ7GzEUciSYm08sskxh0tO        
 zM1fMLFFiELBZXeHNzGy1z4Li/i0fx/nvlZwqVu29fKWZZ6SElPv4x9dfFfvPxXUAK+f        
 cccPZIUnw97TcCNGqz+jEeVaC9kSqtPJqKxR5OQMd9ju7CI0Aj13g8JthKQeUil44KM8        
 h/rAxBNiokc0OAK9DJBRcXOFfbtvvfest7W1YqoKWq+cAKAd293ftcUe0feYKJ0OZMbc        
 WExw==
ARC-Seal: i=1; a=rsa-sha256; s=arcselector9901; d=microsoft.com; cv=none;
 b=frYB95Cl5ec4N6ZAwKfilwbzj6LDy674la8PhMmdwbOgMAjqKauMPtTbSRgXt3H4FENTYa1GzdcSwOf8Ig0E75vjdIpu6FDC4ftngeprlRcv45PsObLrYe4Qr++UJROotdKiREgAXadGBRMjZj9Xoc2cxXKVnlNQc2Rc4x/fYqf6qyoo/zmneooPOULsPOvzX15p5237LBrFVMPvPqMvld+YnDpE1igVsuRi3xL9yxwwFNPJrNYc3gUIHvNGd/pyM5+rUfIX2MLbDOIK+4k+UZ8JKMFxRdIEROKnyxsTqbcfcc3nccfReqcIPZk/oK14fhL+G/VaOlUhZVjIxRPMNg==
ARC-Message-Signature: i=2; a=rsa-sha256; c=relaxed/relaxed; d=google.com; s=arc-20160816;       
 h=content-transfer-encoding:auto-submitted:message-id:subject:date:cc       
  :to:from:mime-version:dkim-signature;       
 bh=l5Q/VKp07PM2V2NeGdKq0Lm/DTxmfWXlHsY7BHC5grY=;       
 b=eyhXuGvJjl0CdClbFb1QPVdBwaU5Bb37Ny7R/m627yg+LjoR12Wm0Ta3ye5Zkf4zjb        
 lEN5zfmlZETUWRhgRcCed17NzROty+pBnYKcxUrU5fjCSxtsqqcid4yCaHNxCbbChpmz        
 UJwMxHIpG4eZ8Q7FpgWm0fTGIaQnq47UPtAVGQfbdodbNHDv6MiSbEQaB5fVp6mT3W7C        
 S3cJA35m+EL7ou93gJXPnUOs6PogF2gLFygKQcuHA9Vo2qeJAJ2vp2gYE7LTSWWFk4FF        
 Fp7x1NKztKFmHPXskCEFtZFBUPrU1rTh3gYa/boXitjErir/TKbHaadEEY2SfwpFXqlv        
 1zpw==
ARC-Message-Signature: i=1; a=rsa-sha256; c=relaxed/relaxed; d=microsoft.com; s=arcselector9901;
 h=From:Date:Subject:Message-ID:Content-Type:MIME-Version:X-MS-Exchange-SenderADCheck; bh=l5Q/VKp07PM2V2NeGdKq0Lm/DTxmfWXlHsY7BHC5grY=;
 b=hEOZehub5S1kTLed4vPfMjCG8mpDOof8GeZt/nkkKwaMMPcgiOtqGLpG0akkyVM6ZzbnotCTupNFg6Ol4Dzq0OQEEYcvsVgNodJBBJdUv6+64A78N3i+Q63PYPP6xDHN93LBny9nQGtAxR2YRChWNriW99vpO0zGLdU+sMK+ZyljT+rEACEUMUK/7kT9Z1eDJL9VndiK/SIA7xSis/YH4YYGVjYvPgAuhA+s/LGi96wGXVjpyWO5AaUA9Hx4y9LwxfaZCeWPMlAd0IgbqJ5di2JmKR4ktdaXCCT7r0mJPhQmJhu6turs6Z4wGrsJyDpbYhtWFuRAyiAoCBXQuEl2dw==
ARC-Authentication-Results: i=2; mx.google.com;       dkim=pass header.i=@executiveship.onmicrosoft.com
 header.s=selector2-executiveship-onmicrosoft-com header.b=GjL0fGgr;      
 arc=pass (i=1 spf=pass spfdomain=executiveship.com);       spf=pass
 (google.com: best guess record for domain of
 postmaster@fujidom2.fujitrading.co.jp designates 218.44.250.29 as permitted
 sender) smtp.helo=FUJIDOM2.fujitrading.co.jp
ARC-Authentication-Results: i=1; mx.microsoft.com 1; spf=pass (sender ip is 121.200.251.220)
 smtp.rcpttodomain=fujitrading.co.jp smtp.mailfrom=executiveship.com;
 dmarc=bestguesspass action=none header.from=executiveship.com; dkim=none
 (message not signed); arc=none
Received-SPF: pass (google.com: best guess record for domain of
 postmaster@fujidom2.fujitrading.co.jp designates 218.44.250.29 as permitted
 sender) client-ip=218.44.250.29;
Received-SPF: Pass (protection.outlook.com: domain of executiveship.com designates
 121.200.251.220 as permitted sender) receiver=protection.outlook.com;
 client-ip=121.200.251.220; helo=mail.executiveship.com;
Authentication-Results: mx.google.com;       dkim=pass header.i=@executiveship.onmicrosoft.com
 header.s=selector2-executiveship-onmicrosoft-com header.b=GjL0fGgr;      
 arc=pass (i=1 spf=pass spfdomain=executiveship.com);       spf=pass
 (google.com: best guess record for domain of
 postmaster@fujidom2.fujitrading.co.jp designates 218.44.250.29 as permitted
 sender) smtp.helo=FUJIDOM2.fujitrading.co.jp
Authentication-Results: spf=pass (sender IP is 121.200.251.220) smtp.mailfrom=executiveship.com;
 fujitrading.co.jp; dkim=none (message not signed)
 header.d=none;fujitrading.co.jp; dmarc=bestguesspass action=none
 header.from=executiveship.com;
X-ASG-Debug-ID: 1576152513-11c674613d1826060001-QbeX01
X-Barracuda-Envelope-From: esmphoenix@executiveship.com
X-Barracuda-Effective-Source-IP: mail-eopbgr1300132.outbound.protection.outlook.com[40.107.130.132]
X-Barracuda-Apparent-Source-IP: 40.107.130.132
DKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed; d=executiveship.onmicrosoft.com;
 s=selector2-executiveship-onmicrosoft-com;
 h=From:Date:Subject:Message-ID:Content-Type:MIME-Version:X-MS-Exchange-SenderADCheck; bh=l5Q/VKp07PM2V2NeGdKq0Lm/DTxmfWXlHsY7BHC5grY=;
 b=GjL0fGgro37up/alpJzGbuWaramjBzGflUEPcqnYfTFB1HEJGB4MEBzyAP+F9TzB+/CE0SVErOZxlId+1O0z3vR/S+tPAwHnqHQGIz2SiCUFoA2nW0eOHb8ztNk2hlZYh+DivuVaVAfkeSkNGS7X6NbmcLs4mN0oGIMozU33z10=
MIME-Version: 1.0
From: esmphoenix@executiveship.com
To: mach-2@fujitrading.co.jp
CC: purchasechn11@executiveship.com, purchasechn11@executiveship.com
Date: Thu, 12 Dec 2019 20:08:23 +0800
Subject: CHOLA HARMONY - RFQ for CHL-19-0098-Ballast valves
X-ASG-Orig-Subj: CHOLA HARMONY - RFQ for CHL-19-0098-Ballast valves
Message-ID: <bc4a3fb52d28417f94e9a526562794bb@ESMSGEXCH13.executiveship.com>
X-Originating-IP: [119.81.76.122]
X-ClientProxiedBy: ESMSGEXCH13.executiveship.com (2002:c8c8:119::c8c8:119) To
 ESMSGEXCH13.executiveship.com (2002:c8c8:119::c8c8:119)
X-EOPAttributedMessage: 0
X-Forefront-Antispam-Report: 	CIP:121.200.251.220;IPV:CAL;SCL:-1;CTRY:SG;EFV:NLI;SFV:NSPM;SFS:(10019020)(39850400004)(396003)(346002)(376002)(136003)(199004)(189003)(356004)(478600001)(26826003)(6666004)(316002)(54906003)(5660300002)(107886003)(66574012)(24736004)(108616005)(2876002)(2906002)(76130400001)(86362001)(16526019)(70586007)(956004)(70206006)(186003)(8676002)(81156014)(81166006)(26005)(8936002)(4326008)(6916009)(336012)(426003)(19000200004);DIR:OUT;SFP:1102;SCL:1;SRVR:HK2PR01MB3170;H:mail.executiveship.com;FPR:;SPF:Pass;LANG:en;PTR:InfoDomainNonexistent;A:0;MX:1;
X-MS-PublicTrafficType: Email
X-MS-Office365-Filtering-Correlation-Id: 7a8af793-cf77-4cae-4900-08d77efc00d3
X-MS-TrafficTypeDiagnostic: 	HK2PR01MB3170:|HK2PR01MB3170:|HK2PR01MB3170:|HK2PR01MB3170:|HK2PR01MB3170:
X-Microsoft-Antispam-PRVS:  <HK2PR01MB3170DC7670FDF63C8C26DF99C1550@HK2PR01MB3170.apcprd01.prod.exchangelabs.com>
X-MS-Exchange-Transport-Forked: True
X-MS-Oob-TLC-OOBClassifiers: OLM:9508;
X-Forefront-PRVS: 0249EFCB0B
X-MS-Exchange-SenderADCheck: 1
X-Microsoft-Antispam: BCL:0;
X-Microsoft-Antispam-Message-Info: 	4nqLGz7JELtXJzqcWO6IAtUjFTeV+5X8RkFEKd95SSwB0xRKjP45d6qMqYnsZSOCsHyR6m+CL4TKJoHJlGepQKxL7eddYRC92bvGB3D+tz4X17pU3V07MX6ok7s1d2aJ4g5EQ7rLbb2MG9CJgyuxlZ82i+usVCZIP7U3JTiPqywIMaJJSlFlDASAkv04bLz+93DsLLxtlwAzHSYmgewGH1s7JbypiFmxibOXsjhP8ZTjKZnMqjN7mXJIjoGOnYWUp5K+tFndlRPLYLm/8rl/214pBU1LKK6Msi2lpmfdEyBqTf5mzM50U8z6K3XKzuW4dcN36bLJfmIuuEvj4jcfHwjy0Q2qOBd2Mja52jxOJc2wnoVrdGg38heqpeFo0/38hYVWnrlz5dydupU0YCtU7dZK1AdFbCDBkgt/S72kw/rQFDOCLsIruhl7e0bKwX8ZQOmqiCd6cq0/6Mub7EY3posEa8il+X7iYWVjJMc/WAgliOHclgWrSvkwGwOT9Hpm
X-OriginatorOrg: executiveship.com
X-MS-Exchange-CrossTenant-OriginalArrivalTime: 12 Dec 2019 12:08:30.5308 (UTC)
X-MS-Exchange-CrossTenant-Network-Message-Id: 7a8af793-cf77-4cae-4900-08d77efc00d3
X-MS-Exchange-CrossTenant-Id: baa38c50-7591-493b-b529-56770cf54231
X-MS-Exchange-CrossTenant-OriginalAttributedTenantConnectingIp: TenantId=baa38c50-7591-493b-b529-56770cf54231;Ip=[121.200.251.220];Helo=[mail.executiveship.com]
X-MS-Exchange-CrossTenant-FromEntityHeader: HybridOnPrem
X-MS-Exchange-Transport-CrossTenantHeadersStamped: HK2PR01MB3170
X-Barracuda-Connect: mail-eopbgr1300132.outbound.protection.outlook.com[40.107.130.132]
X-Barracuda-Start-Time: 1576152513
X-Barracuda-URL: https://192.168.200.10:443/cgi-mod/mark.cgi
X-Barracuda-Scan-Msg-Size: 1684
X-Barracuda-Spam-Score: 3.56
X-Barracuda-Spam-Status: No, SCORE=3.56 using global scores of TAG_LEVEL=1000.0
 QUARANTINE_LEVEL=1000.0 KILL_LEVEL=8.0 tests=ADVANCE_FEE_1, ADVANCE_FEE_2,
 ADVANCE_FEE_2_2, BSF_SC0_SA083, MAILTO_TO_SPAM_ADDR, NO_REAL_NAME
X-Barracuda-Spam-Report: Code version 3.2, rules version 3.2.3.78610	Rule breakdown below	 pts rule
 name              description	---- ----------------------
 --------------------------------------------------	0.00 NO_REAL_NAME        
   From: does not include a real name	0.00 ADVANCE_FEE_1          Appears to
 be advance fee fraud (Nigerian 419)	0.00 MAILTO_TO_SPAM_ADDR    Includes a
 link to a likely spammer email	0.01 ADVANCE_FEE_2          Appears to be
 advance fee fraud (Nigerian 419)	2.05 ADVANCE_FEE_2_2        Appears to be
 advance fee fraud (Nigerian 419)	1.50 BSF_SC0_SA083          Custom Rule
 SA083
X-MIMETrack: Itemize by SMTP Server on FUJIDOM2/Fuji Trading(Release 10.0.1FP2 HF10|June
 21, 2019) at 2019/12/12 21:08:34,	Serialize by Router on FUJIDOM2/Fuji
 Trading(Release 10.0.1FP2 HF10|June 21, 2019) at 2019/12/12
 21:08:35,	Serialize complete at 2019/12/12 21:08:35
X-TNEFEvaluated: 1
X-MailFwdBy: CN=MACH-2/O=Fuji Trading
Auto-Submitted: auto-forwarded
Content-Transfer-Encoding: base64
Content-Type: text/plain; charset="us-ascii"
X-ECM-Milter: Whitelisted
X-ECM-Milter-ID: 0e15e37073a85199233f7a7b8162e3d5
X-NetcoreSecure6-ECMScanner-Information: Please contact Netcore Support for more information
X-NetcoreSecure6-MailScanner-ID: 6F6859C15.A381F
X-NetcoreSecure6-ECMScanner: Found to be clean
X-NetcoreSecure6-ECMScanner-SpamCheck: 
X-NetcoreSecure6-ECMScanner-From: mach-2+caf_=fuji_mach=lighthouse-esolutions.com@msdmail.fujitrading.co.jp

                                                                                                          �      F                                                �                                                             �                                                             IPM.Note                                                        CHOLA HARMONY - RFQ for CHL-19-0098-Ballast valves              esmphoenix@executiveship.com                                    esmphoenix@executiveship.com                                    CHOLA HARMONY - RFQ for CHL-19-0098-Ballast valves                  �+����n �T  �e s m p h o e n i x @ e x e c u t i v e s h i p . c o m   S M T P   e s m p h o e n i x @ e x e c u t i v e s h i p . c o m                                             esmphoenix@executiveship.com                                    SMTP:ESMPHOENIX@EXECUTIVESHIP.COM                               SMTP                                                            esmphoenix@executiveship.com                                    purchasechn11@executiveship.com;purchasechn11@executiveship.com mach-2@fujitrading.co.jp                                        CHOLA HARMONY - RFQ for CHL-19-0098-Ballast valves              This is an automated message. DO NOT REPLY to esmphoenix@executiveship.com. Kindly use the "reply all" button if you are responding to this message.

FUJI TRADING CO LTD             CHL-19-0098

Dear Sir

EXECUTIVE SHIP MANAGEMENT PTE LTD hereby requests you to provide your BEST quotation for the following items to be delivered to our vessel
CHOLA HARMONY

Request your IT department to kindly allow access to this URL for submitting quotes.
Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser
"<https://apps2.southnests.com:443/PhoenixTelerik/Purchase/PurchaseQuotationItems.aspx?SESSIONID=CBDE44E2-D61C-EA11-80D9-065916D1083C&STOCKTYPE=SPARE>"


We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.

Note: In our continual effort to keep correct records of your address and contact information, we appreciate your time to verify and correct it where necessary. Please click on the link below to view/correct the address.

"<https://apps2.southnests.com:443/PhoenixTelerik/Purchase/PurchaseVendorAddressEdit.aspx?sessionid=bf81aab0-8b9d-e011-b680-00215a5129d6>"


Thank you,

Yokeshwaran  K
For and on behalf of EXECUTIVE SHIP MANAGEMENT PTE LTD

(As Agents for Owners)

Contact: purchasechn11@executiveship.com


This is an automated message.
If you need personal attention, use "reply all" to get your communication across to an email id that is monitored.
Please note esmphoenix@executiveship.com is NOT monitored.

                                                D  D  MELA    {\rtf1\ansi\fbidis\ansicpg1252\deff0\deftab720\fromtext{\fonttbl{\f0\fswiss\fcharset0 Times New Roman;}{\f1\fswiss\fcharset2
Symbol;}}
{\colortbl;\red192\green192\blue192;}
\viewkind5\viewscale100
{\*\bkmkstart BM_BEGIN}\pard\plain\fs20\f0{This is an automated message. DO NOT REPLY to esmphoenix@executiveship.com. Kindly use the "reply all" button if you are responding to this message.\line
\line
FUJI TRADING CO LTD             CHL-19-0098\line
\line
Dear Sir\line
\line
EXECUTIVE SHIP MANAGEMENT PTE LTD hereby requests you to provide your BEST quotation for the following items to be delivered to our vessel\line
CHOLA HARMONY\line
\line
Request your IT department to kindly allow access to this URL for submitting quotes.\line
Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser\line
"<https://apps2.southnests.com:443/PhoenixTelerik/Purchase/PurchaseQuotationItems.aspx?SESSIONID=CBDE44E2-D61C-EA11-80D9-065916D1083C&STOCKTYPE=SPARE>"\line
\line
\line
We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.\line
\line
Note: In our continual effort to keep correct records of your address and contact information, we appreciate your time to verify and correct it where necessary. Please click on the link below to view/correct the address.\line
\line
"<https://apps2.southnests.com:443/PhoenixTelerik/Purchase/PurchaseVendorAddressEdit.aspx?sessionid=bf81aab0-8b9d-e011-b680-00215a5129d6>"\line
\line
\line
Thank you,\line
\line
Yokeshwaran  K\line
For and on behalf of EXECUTIVE SHIP MANAGEMENT PTE LTD\line
\line
(As Agents for Owners)\line
\line
Contact: purchasechn11@executiveship.com\line
\line
\line
This is an automated message.\line
If you need personal attention, use "reply all" to get your communication across to an email id that is monitored.\line
Please note esmphoenix@executiveship.com is NOT monitored.\line
\line
}}
                                                        <bc4a3fb52d28417f94e9a526562794bb@ESMSGEXCH13.executiveship.com>                              @ 0   %��f��@ 0   %��f��@ 9    ������ �           �          �?   �N                            4   y          	                 =              3       7    3           �                  "    �   �    �                      �                     @    ������ p    3       B           e              @                 }    �7      5   A      	   H   � 6            d                             �                                                                �+����n �T  �m a c h - 2 @ f u j i t r a d i n g . c o . j p   S M T P   m a c h - 2 @ f u j i t r a d i n g . c o . j p                                                             mach-2@fujitrading.co.jp                                        SMTP                                                            mach-2@fujitrading.co.jp                                        SMTP:MACH-2@FUJITRADING.CO.JP                                           �       ��   �    � 0          0          0         0       �            0                                                                     �                                                                �+����n �T  �p u r c h a s e c h n 1 1 @ e x e c u t i v e s h i p . c o m   S M T P   p u r c h a s e c h n 1 1 @ e x e c u t i v e s h i p . c o m                                 purchasechn11@executiveship.com                                 SMTP                                                            purchasechn11@executiveship.com                                 SMTP:PURCHASECHN11@EXECUTIVESHIP.COM                                    �       ��   �    � 0           0          0          0   %    �            0                                                                    �                                                                �+����n �T  �p u r c h a s e c h n 1 1 @ e x e c u t i v e s h i p . c o m   S M T P   p u r c h a s e c h n 1 1 @ e x e c u t i v e s h i p . c o m                                 purchasechn11@executiveship.com                                 SMTP                                                            purchasechn11@executiveship.com                                 SMTP:PURCHASECHN11@EXECUTIVESHIP.COM                                    �       ��   �    � 0           0          0          0   %    �            0                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ��������������������������������	   
   ��������������������������������                                           !   "   #   $   %   &   '   (   )   *   +   ,   ����.   /   0   1   2   3   4   5   6   7   8   9   :   ;   <   =   >   ?   @   A   B   C   D   E   F   G   H   I   J   K   L   M   N   ��������Q   R   S   T   U   V   W   X   ��������[   \   ��������������������b   c   ��������f   g   ��������������������m   n   ��������q   r   ��������������������x   y   ����                        R o o t   E n t r y                                              ��������        �      F                       �      _ _ n a m e i d _ v e r s i o n 1 . 0                           ( ����                                                      _ _ s u b s t g 1 . 0 _ 0 0 0 2 0 1 0 2                         * ����   ����                                               _ _ s u b s t g 1 . 0 _ 0 0 0 3 0 1 0 2                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 0 0 4 0 1 0 2                         * ����   ����                                    ����        _ _ s u b s t g 1 . 0 _ 1 0 1 2 0 1 0 2                         * ������������                                              _ _ s u b s t g 1 . 0 _ 0 0 1 A 0 0 1 E                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 0 3 7 0 0 1 E                         * ����   ����                                       2       _ _ s u b s t g 1 . 0 _ 0 0 3 D 0 0 1 E                         * ����	   ����                                    ����        _ _ s u b s t g 1 . 0 _ 0 0 4 2 0 0 1 E                         * ����
   ����                                              _ _ s u b s t g 1 . 0 _ 0 0 6 4 0 0 1 E                         * ����   ����                                    ����        _ _ s u b s t g 1 . 0 _ 0 0 6 5 0 0 1 E                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 0 7 0 0 0 1 E                         * ����   ����                                       2       _ _ s u b s t g 1 . 0 _ 0 0 7 D 0 0 1 E                         * ����   ����                                        �7      _ _ s u b s t g 1 . 0 _ 0 C 1 9 0 1 0 2                         * ����   ����                                       �       _ _ s u b s t g 1 . 0 _ 0 C 1 A 0 0 1 E                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 C 1 D 0 1 0 2                         * ����   ����                                       "       _ _ s u b s t g 1 . 0 _ 0 C 1 E 0 0 1 E                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 C 1 F 0 0 1 E                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 E 0 2 0 0 1 E                         * ����   ����                                    ����        _ _ s u b s t g 1 . 0 _ 0 E 0 3 0 0 1 E                         * ����   ����                                       ?       _ _ s u b s t g 1 . 0 _ 0 E 0 4 0 0 1 E                         * ����   ����                                              _ _ s u b s t g 1 . 0 _ 0 E 1 D 0 0 1 E                         * ����   ����                                       2       _ _ s u b s t g 1 . 0 _ 1 0 0 0 0 0 1 E                         * ����   ����                                       �      _ _ s u b s t g 1 . 0 _ 1 0 0 9 0 1 0 2                         * ����   ����                                    -   H      _ _ s u b s t g 1 . 0 _ 1 0 3 5 0 0 1 E                         * ����   ����                                    O   @       _ _ p r o p e r t i e s _ v e r s i o n 1 . 0                   0 ����   ����                                    P   0      _ _ r e c i p _ v e r s i o n 1 . 0 _ # 0 0 0 0 0 0 0 0         : ����#                                                      _ _ s u b s t g 1 . 0 _ 0 F F 6 0 1 0 2                         * ����   ����                                    Y          _ _ s u b s t g 1 . 0 _ 0 F F F 0 1 0 2                         * ����   ����                                    Z   �       _ _ s u b s t g 1 . 0 _ 3 0 0 1 0 0 1 E                         * ����   ����                                    ]          _ _ s u b s t g 1 . 0 _ 3 0 0 2 0 0 1 E                         * ����    ����                                    ^          _ _ s u b s t g 1 . 0 _ 3 0 0 3 0 0 1 E                         * ����!   ����                                    _          _ _ s u b s t g 1 . 0 _ 3 0 0 B 0 1 0 2                         * ����"   ����                                    `          _ _ p r o p e r t i e s _ v e r s i o n 1 . 0                   0 ������������                                    a   �       _ _ r e c i p _ v e r s i o n 1 . 0 _ # 0 0 0 0 0 0 0 1         : ����+   $                                                   _ _ s u b s t g 1 . 0 _ 0 F F 6 0 1 0 2                         * ����%   ����                                    d          _ _ s u b s t g 1 . 0 _ 0 F F F 0 1 0 2                         * ����&   ����                                    e   �       _ _ s u b s t g 1 . 0 _ 3 0 0 1 0 0 1 E                         * ����'   ����                                    h          _ _ s u b s t g 1 . 0 _ 3 0 0 2 0 0 1 E                         * ����(   ����                                    i          _ _ s u b s t g 1 . 0 _ 3 0 0 3 0 0 1 E                         * ����)   ����                                    j          _ _ s u b s t g 1 . 0 _ 3 0 0 B 0 1 0 2                         * ����*   ����                                    k   %       _ _ p r o p e r t i e s _ v e r s i o n 1 . 0                   0 ������������                                    l   �       _ _ r e c i p _ v e r s i o n 1 . 0 _ # 0 0 0 0 0 0 0 2         : ��������,                                                   _ _ s u b s t g 1 . 0 _ 0 F F 6 0 1 0 2                         * ����-   ����                                    o          _ _ s u b s t g 1 . 0 _ 0 F F F 0 1 0 2                         * ����.   ����                                    p   �       _ _ s u b s t g 1 . 0 _ 3 0 0 1 0 0 1 E                         * ����/   ����                                    s          _ _ s u b s t g 1 . 0 _ 3 0 0 2 0 0 1 E                         * ����0   ����                                    t          _ _ s u b s t g 1 . 0 _ 3 0 0 3 0 0 1 E                         * ����1   ����                                    u          _ _ s u b s t g 1 . 0 _ 3 0 0 B 0 1 0 2                         * ����2   ����                                    v   %       _ _ p r o p e r t i e s _ v e r s i o n 1 . 0                   0 ������������                                    w   �                                                                          ������������                                                                        	   
                                                      ����             !   "   #   $   %   &   '   (   )   *   +   ��������.   /   0   1   2   3   4   5   6   7   8   9   ��������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������