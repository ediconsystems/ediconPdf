@echo Vytvoreni dokladu Edicon
@pause
EdiconPdf.exe -c 0120010001_orig.pdf 0120010001_orig.isdoc 0120010001.pdf

@echo.
@echo Ziskani dat z dokladu Edicon s ulozenim do souboru
@pause
EdiconPdf.exe -x 0120010001.pdf 0120010001_extracted.isdoc

@echo.
@echo Vypis dat z dokladu Edicon na obrazovku
@pause
EdiconPdf.exe -x 0120010001.pdf

@echo.
@echo Pokus o ziskani dat z pdf bez prilohy
@pause
EdiconPdf.exe -x 0120010001_orig.pdf

@pause
