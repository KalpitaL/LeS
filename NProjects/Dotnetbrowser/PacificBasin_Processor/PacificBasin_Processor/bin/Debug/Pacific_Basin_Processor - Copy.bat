TITLE  COPY PASIFIC BASIN FILES to 203-VPC4 server started at - %date% %time%

@ ECHO OFF
net use \\119.81.96.248 /USER:Administrator "KSYhR77q" 
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
set "year=%YYYY%" & set "month=%MM%"
set "month-num=%date:~3,2%"
echo path : %year%\%month%
if %month%==01 set month=jan
if %month%==02 set month=feb
if %month%==03 set month=mar
if %month%==04 set month=apr
if %month%==05 set month=may
if %month%==06 set month=jun
if %month%==07 set month=jul
if %month%==08 set month=aug
if %month%==09 set month=sep
if %month%==10 set month=oct
if %month%==11 set month=nov
if %month%==12 set month=dec


::echo Copy Pacific Basin Neko Quote files 

::xcopy \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\NEKO_Buyers\WEB_Buyers\Pacific\Outbox\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\NEKO_Buyers\WEB_Buyers\Pacific\Outbox\Backup /Y /I
::move /Y \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\NEKO_Buyers\WEB_Buyers\Pacific\Outbox\*.* C:\LeS\eSupplierPortal\LeS_MTML_Quotes\PacificBasin\SM0372_WEB

::xcopy \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\NEKO_Buyers\WEB_Buyers\EIMSKIP\Outbox\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\NEKO_Buyers\WEB_Buyers\EIMSKIP\Outbox\Backup /Y /I
::move /Y \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\NEKO_Buyers\WEB_Buyers\EIMSKIP\Outbox\*.* C:\LeS\eSupplierPortal\LeS_MTML_Quotes\PacificBasin\SM0695_WEB

::echo Copy Pacific Basin WSS Quote files
::xcopy \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\OUTBOX\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\OUTBOX\Backup /Y /I
::move /Y \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\OUTBOX\*.* C:\LeS\eSupplierPortal\LeS_MTML_Quotes\PacificBasin\SM0668_WEB

::xcopy \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\Norbulk\OUTBOX\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\Norbulk\OUTBOX\Backup /Y /I
::move /Y \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\Norbulk\OUTBOX\*.* C:\LeS\eSupplierPortal\LeS_MTML_Quotes\PacificBasin\SM0711_WEB

::xcopy \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\ASP\OUTBOX\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\ASP\OUTBOX\Backup /Y /I
::move /Y \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\ASP\OUTBOX\*.* C:\LeS\eSupplierPortal\LeS_MTML_Quotes\PacificBasin\SM0518_WEB

::xcopy \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\X-PRESS\OUTBOX\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\X-PRESS\OUTBOX\Backup /Y /I
::move /Y \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\WebScriptBuyers\DNVGL_WSS\X-PRESS\OUTBOX\*.* C:\LeS\eSupplierPortal\LeS_MTML_Quotes\PacificBasin\SM3171_WEB


c:
cd C:\LeS\LeS_Processors\Pacific_Basin_Processor\
PacificBasin_Processor.exe

xcopy C:\LeS\eSupplierPortal\ATTACHMENTS\*.* C:\LeS\eSupplierPortal\ATTACHMENTS\Backup /Y /I
move /Y C:\LeS\eSupplierPortal\ATTACHMENTS\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\ESUPPLIER_ATTACHMENTS\%YYYY%\%MM%

::echo AuditLog
::COPY C:\LeS\eSupplierPortal\AuditLog\*.* C:\LeS\eSupplierPortal\AuditLog\Backup /Y
::MOVE /Y C:\LeS\eSupplierPortal\AuditLog\*.* \\119.81.96.248\c$\ftp\eSupplierAudit

xcopy C:\LeS\LeS_Processors\Pacific_Basin_Processor\ScreenShots\*.* C:\LeS\LeS_Processors\Pacific_Basin_Processor\ScreenShots\Backup /Y /I
move /Y C:\LeS\LeS_Processors\Pacific_Basin_Processor\ScreenShots\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\ESUPPLIER_ATTACHMENTS\%YYYY%\%MM%

xcopy C:\LeS\LeS_Processors\Pacific_Basin_Processor\XML\*.* C:\LeS\LeS_Processors\Pacific_Basin_Processor\XML\Backup /Y /I
move /Y C:\LeS\LeS_Processors\Pacific_Basin_Processor\XML\*.* \\119.81.96.248\c$\SHIPMASTER\eSupplierPortal\LES_XML_INBOX





