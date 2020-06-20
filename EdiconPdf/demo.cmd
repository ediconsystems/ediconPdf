@echo Vytvoreni dokladu Edicon
@pause
EdiconPdf.exe -c 00030000071908_orig.pdf 00030000071908_orig.isdoc 00030000071908_new.pdf

@echo.
@echo ziskani dat z dokladu Edicon s ulozenim do souboru
@pause
EdiconPdf.exe -x 00030000071908_new.pdf 00030000071908_extracted.isdoc

@echo.
@echo Vypis dat z dokladu Edicon na obrazovku
@pause
EdiconPdf.exe -x 00030000071908_new.pdf

@pause
