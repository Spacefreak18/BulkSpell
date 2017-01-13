# BulkSpell #
https://github.com/ZitZ/BulkSpell

C# dot net application to analyze spell check results from multiple documents of various types, with user-selectable customized dictionaries. Simply point to a directory, select a dictionary, and await your results. BulkSpell also provides a list of unique misspellings for the files selected by clicking "Reverse List".

## Nuget Libraries Used ##

### iTextSharp ###
<https://github.com/itext/itextsharp>

Used to extract text from Pdf files on the fly.

### NetSpell ###
<https://github.com/cristianst85/NetSpell>
<http://www.loresoft.com/The-NetSpell-project>

The spell checking library being used. It is a 100% dot net solution. The github repository is a fork, not from the original author.


ToDo:
### Mime-Detective ###
<https://github.com/Muraad/Mime-Detective>

Use for properly detecting the file type without relying on the file extension. Currently, there is a hard coded mime detecting solution in use.


