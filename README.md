# Crozzle
This Repository holds the first version of Crozzle Application. It reads files, valid files and show crozzle with its score.

Introduction

The Crozzle App is a windows forms application developed using C#. The working of this application is based on requirements set in assignment task document.
I had tested this application and its working using the three set of files in Marking files folder. You will see a Marking files folder with three sub-folders each containing a set of files and location for log file. The name of the log file in all configuration files was the same, I have not made any changes to the log file name, but for the sake of marker’s readability created three sub-folders.
The Crozzle Application requires a Crozzle file, which must have a name or path of the Configuration file and Word List file.
Here is the step by step working of the application.
1.	Open File menu, select Open Crozzle option. You will be asked to select the Crozzle file. You only need to select Crozzle file and not Configuration and Word List file.
2.	Crozzle will be displayed on one form, errors if any will be displayed in another form.
3.	One can click on Help in Menu to get more information about the application.
4.	If no errors appear until now, the three files are Valid.
5.	Click on Score for Menu and click on Calculate Score option.
6.	It will do necessary validation and then the correct score will be displayed just below the Crozzle.
7.	If you want to exit the application, click on File on Menu and click on Exit Option.
8.	If any of the three files is found not valid, application will not further validate/ calculate score.


Body
This provides the understanding of the Crozzle application using three set of input files. The usage of these three sets is depicted by the Sections below: -

Section A
This section shows the working of Crozzle produced 
 
Figure 1 Marking Test 1 - Crozzle
Here is the window which shows all the errors. It is empty now because there were no errors.

 
Figure 2 Marking Test 1 - Error Form

 
Figure 3 Marking Test 1 - Log File


Section B
This Section shows the use of second set of data in Marking Files folder. In this case, there are errors while loading the initial configuration. Note that the Crozzle is invalid but I am still displaying it because it gives indication to solving visible errors like this

 
Figure 4 Marking Test - 2 Crozzle

Error detection becomes easy If Crozzle is displayed regardless of validity.
The errors below are different from those which are found when we try to get the Score. These errors show that the three data files are invalid.
 
Figure 5 Marking Test 2 - Error Form

User may try to calculate the score but it will not be successful. Note how the score is displayed below. 
Figure 6 Marking 2 Form - Invalid Score
 
Figure 7 Marking Test 2 - Log File
Section C
In Marking Test 3, the configuration file, Crozzle file and word list files were Valid but after validation it was found that Outcome (Crozzle) is invalid and hence score was not calculated. Figure 8 Marking Test 3 – Crozzle
 
Figure 9 Marking Test 3 - Error form
 
Figure 10 Marking Test 3 - Log File


Appendix
Appendix A
Marking Test 1 - Crozzle File
// File dependencies.
// Configuration file.
CONFIGURATION_FILE=".\Marking 1 Configuration.txt"

// Word list file.
WORDLIST_FILE=".\Marking 1 Wordlist.txt"

// Crozzle Size.
// The number of rows and columns.
ROWS=10       // This Crozzle will have 10 rows.
COLUMNS=10

// Word Data.
// The horizontal rows containing words.
ROW=3,ANGELA,3
ROW=7,CATO,3
ROW=6,ED,1
ROW=8,GARY,6
ROW=1,GRACE,1
ROW=9,JACKIE,1
ROW=2,LE,3
ROW=6,LE,9
ROW=5,LEAH,7
ROW=6,MARY,4
ROW=10,RODDY,6
ROW=4,RONA,1
ROW=1,TOM,8
ROW=4,VIC,6

// The vertical rows containing words.
COLUMN=9,AL,5
COLUMN=3,ALAN,1
COLUMN=6,BEV,2
COLUMN=4,CENA,1
COLUMN=1,GEORGE,1
COLUMN=7,LILY,3
COLUMN=4,MARK,6
COLUMN=5,MAT,5
COLUMN=10,MATTHEW,1
COLUMN=2,PAM,8
COLUMN=6,ROGER,6
COLUMN=8,ROD,8
COLUMN=8,TRACE,1
COLUMN=10,TY,9


Marking Test 1 – Configuration File
// Log File Configurations.
// The default log file name.
LOGFILE_NAME="log.txt"   // log file   

// Word List Configurations.
// Limits on the size of the word list.
MINIMUM_NUMBER_OF_UNIQUE_WORDS=10
MAXIMUM_NUMBER_OF_UNIQUE_WORDS=1000

// Crozzle Output Configurations.
INVALID_CROZZLE_SCORE="INVALID CROZZLE"
UPPERCASE=true
STYLE="<style> table, td { border: 1px solid black; border-collapse: collapse; } td { width:24px; height:18px; text-align: center; } </style>"

BGCOLOUR_EMPTY_TD=#777777
BGCOLOUR_NON_EMPTY_TD=#ffffff

// Crozzle Configurations.
// Limits on the size of the Crozzle grid.
MINIMUM_NUMBER_OF_ROWS=8
MAXIMUM_NUMBER_OF_ROWS=80
MINIMUM_NUMBER_OF_COLUMNS=10
MAXIMUM_NUMBER_OF_COLUMNS=100

// Limits on the number of horizontal words and
// vertical words in a crozzle.
MINIMUM_HORIZONTAL_WORDS=1
MAXIMUM_HORIZONTAL_WORDS=100
MINIMUM_VERTICAL_WORDS=1
MAXIMUM_VERTICAL_WORDS=100

// Limits on the number of 
// intersecting vertical words for each horizontal word, and
// intersecting horizontal words for each vertical word.
MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS=1
MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS=100
MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS=1
MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS=100

// Limits on duplicate words in the Crozzle.
MINIMUM_NUMBER_OF_THE_SAME_WORD=1
MAXIMUM_NUMBER_OF_THE_SAME_WORD=2

// Limits on the number of valid word groups.
MINIMUM_NUMBER_OF_GROUPS=1
MAXIMUM_NUMBER_OF_GROUPS=1

// Scoring Configurations
// The number of points per word within the crozzle.
POINTS_PER_WORD=10

// Points per letter that is at the intersection of
// a horizontal and vertical word within the crozzle.
INTERSECTING_POINTS_PER_LETTER="A=1,B=2,C=2,D=2,E=1,F=4,G=4,H=4,I=1,J=8,K=8,L=8,M=8,N=8,O=1,P=16,Q=16,R=16,S=16,T=16,U=1,V=32,W=32,X=64,Y=64,Z=128"

// Points per letter that is not at the intersection of
// a horizontal and vertical word within the crozzle.
NON_INTERSECTING_POINTS_PER_LETTER="A=0,B=0,C=0,D=0,E=0,F=0,G=0,H=0,I=0,J=0,K=0,L=0,M=0,N=0,O=0,P=0,Q=0,R=0,S=0,T=0,U=0,V=0,W=0,X=0,Y=0,Z=0"

Marking Test 1 – Word List File
AL,ALAN,ANGELA,BETTY,BEV,BILL,BRENDA,CATO,CENA,ED,FRED,GARY,GEORGE,GRACE,HARRY,JACKIE,JESSICA,JILL,JOHN,LARRY,LE,LEAH,LILY,MARK,MARY,MAT,MATTHEW,OSCAR,PAM,ROBERT,ROGER,ROD,RODDY,RONA,TOM,TRACE,TY,VIC,WENDY,WALTER

Appendix B
Marking Test 2 – Crozzle File

// File dependencies.
// Configuraton file.
CONFIGURATION_FILE=".\Marking 2 Configuration.txt"

// Word list file.
WORDLIST_FILE=".\Marking 2 Wordlist.txt"

// Crozzle Size.
// The number of rows and columns.
ROWS=10       // This crozzle will have 10 rows.
COLUMNS=10

// Word Data.
// The horizontal rows containing words.
=3,ANGELA,3      // error 1
ROW=,CATO,3      // error 2
ROW=2,$#@$#@,3   // error 3
ROW=5,LEAH,XYZ   // error 4
ROW=6,,1         // error 5
ROW=8,GARY,      // error 6
ROW=12,GRACE,1   // error 7
ROW=ABC,LE,9     // error 8
ROW=1,TOM,8
ROW=4,RONA,1
ROW=4,VIC,6
ROW=6,MARY,4
ROW=9,JACKIE,1
ROW=10,RODDY,6

// The vertical rows containing words.
=3,ALAN,123        // error 9
COLUMN=,CENA,1     // error 10
COLUMN=5,???,5     // error 11
COLUMN=7.5,LILY,3  // error 12
COLUMN=9,AL,9.5    // error 13
COLUMN=1,GEORGE,1
COLUMN=2,PAM,8
COLUMN=4,MARK,6
COLUMN=6,BEV,2
COLUMN=6,ROGER,6
COLUMN=8,TRACE,1
COLUMN=8,ROD,8
COLUMN=10,MATTHEW,1
COLUMN=10,TY,9



Marking Test 2 – Configuration File

// Log File Configurations.
// The default log file name.
LOGFILE_NAME="log.txt"   // log file   

// Word List Configurations.
// Limits on the size of the word list.
MINIMUM_NUMBER_OF_UNIQUE_WORDS=abc  // error 1: abc
MAXIMUM_NUMBER_OF_UNIQUE_WORDS=1000

// Crozzle Output Configurations.
INVALID_CROZZLE_SCORE="INVALID CROZZLE"
UPPERCASE=TRUE                      // error 2: TRUE
STYLE="<style> table, td { border: 1px solid black; border-collapse: collapse; } td { width:24px; height:18px; text-align: center; } </style>"

BGCOLOUR_EMPTY_TD=#7777777          // error 3: 7 digits
BGCOLOUR_NON_EMPTY_TD=#fffffff      // error 4: 7 digits

// Crozzle Configurations.
// Limits on the size of the crozzle grid.
MAXIMUM_NUMBER_OF_ROWS=8   // error 5: MINIMUM_NUMBER_OF_ROWS missing
MAXIMUM_NUMBER_OF_ROWS=80  // error 6: duplicate AXIMUM_NUMBER_OF_ROWS
MINIMUM_NUMBER_OF_COLUMNS=10
MAXIMUM_NUMBER_OF_COLUMNS=100

// Limits on the number of horizontal words and
// vertical words in a crozzle.
MINIMUM_HORIZONTAL_WORDS=1
MAXIMUM_HORIZONTAL_WORDS=100
MINIMUM_VERTICAL_WORDS=1
MAXIMUM_VERTICAL_WORDS=100

// Limits on the number of 
// intersecting vertical words for each horizontal word, and
// intersecting horizontal words for each vertical word.
MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS=1
MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS=100
MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS=1
MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS=100

// Limits on duplicate words in the crozzle.
MINIMUM_NUMBER_OF_THE_SAME_WORD=1
MAXIMUM_NUMBER_OF_THE_SAME_WORD=2

// Limits on the number of valid word groups.
// MINIMUM_NUMBER_OF_GROUPS=1   // error 7: keyword missing
MAXIMUM_NUMBER_OF_GROUPS=3

// Scoring Configurations
// The number of points per word within the crozzle.
POINTS_PER_WORD=10
POINTS_PER_WORD=123  // error 8: duplicate POINTS_PER_WORD


// Points per letter that is at the intersection of
// a horizontal and vertical word within the crozzle.
INTERSECTING_POINTS_PER_LETTER="A=1,B=2,C=2,D=2,E=1,F=4,G=4,H=4,I=1,J=8,K=8,L=8,M=8,N=8,O=1,P=16,Q=16,R=16,S=16,T=16,U=1,V=32,W=32,X=64,Y=64,Z=128"

// Points per letter that is not at the intersection of
// a horizontal and vertical word within the crozzle.
NON_INTERSECTING_POINTS_PER_LETTER="B=0,C=0,D=0,E=0,F=0,G=0,H=0,I=0,J=0,K=0,L=0,M=0,N=0,O=0,P=0,Q=0,R=0,S=0,T=0,U=0,V=0,W=0,X=0,Y=0,Z=0,@=0" 
// errors 9 & 10: A missing, @ invalid

Marking Test 2 – Word List File

AL,ALAN,ANGELA,BETTY,BEV,BILL,BRENDA,CATO,CENA,ED,FRED,GARY,GEORGE,GRACE,HARRY,JACKIE,JESSICA,JILL,JOHN,LARRY,LE,LEAH,LILY,MARK,MARY,MAT,MATTHEW,OSCAR,PAM,ROBERT,ROGER,ROD,RODDY,RONA,TOM,TRACE,TY,WENDY,VIC.,WALTER,"WILMA",WADE,O'NEIL,WALT,123,,WAYNE,WALDO,WAYNE,WILL,????,WILLOW,PRENTICE-HALL,WESLEY,JAMES COOK,WANETA,HUNGRY
JACKS,QWILSONQ,WILSON

Appendix C
Marking Test 3 – Crozzle File

// File dependencies.
// Configuraton file.
CONFIGURATION_FILE=".\Marking 3 Configuration.txt"

// Word list file.
WORDLIST_FILE=".\Marking 3 Wordlist.txt"

// Crozzle Size.
// The number of rows and columns.
ROWS=10       // This crozzle will have 10 rows.
COLUMNS=10

// Word Data.
// The horizontal rows containing words.
ROW=1,GRACE,1
ROW=2,LE,3
ROW=3,ANGELA,3
ROW=4,RONA,1
ROW=4,VIC,6
ROW=5,LE,7
ROW=6,ED,1
ROW=6,MARY,4
ROW=6,LE,9
ROW=7,CATO,3
ROW=7,LEW,8
ROW=9,JACK,1
ROW=9,LE,7
ROW=10,FREDDY,5


// The vertical rows containing words.
COLUMN=1,GEORGE,1
COLUMN=2,MAT,8
COLUMN=3,ALAN,1
COLUMN=4,CENA,1
COLUMN=4,MARK,6
COLUMN=5,MAT,5
COLUMN=6,BEV,2
COLUMN=6,ROD,6
COLUMN=7,LILY,3
COLUMN=7,LE,9
COLUMN=8,TRACE,1
COLUMN=8,ED,9
COLUMN=9,LEO,6
COLUMN=10,MATTHEW,1
COLUMN=10,TY,9



Marking Test 3 – Configuration File

// Log File Configurations.
// The default log file name.
LOGFILE_NAME="log.txt"   // log file   

// Word List Configurations.
// Limits on the size of the word list.
MINIMUM_NUMBER_OF_UNIQUE_WORDS=10
MAXIMUM_NUMBER_OF_UNIQUE_WORDS=1000

// Crozzle Output Configurations.
INVALID_CROZZLE_SCORE="INVALID CROZZLE"
UPPERCASE=true
STYLE="<style> table, td { border: 1px solid black; border-collapse: collapse; } td { width:24px; height:18px; text-align: center; } </style>"

BGCOLOUR_EMPTY_TD=#777777
BGCOLOUR_NON_EMPTY_TD=#ffffff

// Crozzle Configurations.
// Limits on the size of the crozzle grid.
MINIMUM_NUMBER_OF_ROWS=8
MAXIMUM_NUMBER_OF_ROWS=80
MINIMUM_NUMBER_OF_COLUMNS=10
MAXIMUM_NUMBER_OF_COLUMNS=100

// Limits on the number of horizontal words and
// vertical words in a crozzle.
MINIMUM_HORIZONTAL_WORDS=1
MAXIMUM_HORIZONTAL_WORDS=10
MINIMUM_VERTICAL_WORDS=1
MAXIMUM_VERTICAL_WORDS=100

// Limits on the number of 
// intersecting vertical words for each horizontal word, and
// intersecting horizontal words for each vertical word.
MINIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS=1
MAXIMUM_INTERSECTIONS_IN_HORIZONTAL_WORDS=4
MINIMUM_INTERSECTIONS_IN_VERTICAL_WORDS=1
MAXIMUM_INTERSECTIONS_IN_VERTICAL_WORDS=4

// Limits on duplicate words in the crozzle.
MINIMUM_NUMBER_OF_THE_SAME_WORD=1
MAXIMUM_NUMBER_OF_THE_SAME_WORD=3

// Limits on the number of valid word groups.
MINIMUM_NUMBER_OF_GROUPS=2
MAXIMUM_NUMBER_OF_GROUPS=2

// Scoring Configurations
// The number of points per word within the crozzle.
POINTS_PER_WORD=10

// Points per letter that is at the intersection of
// a horizontal and vertical word within the crozzle.
INTERSECTING_POINTS_PER_LETTER="A=1,B=2,C=2,D=2,E=1,F=4,G=4,H=4,I=1,J=8,K=8,L=8,M=8,N=8,O=1,P=16,Q=16,R=16,S=16,T=16,U=1,V=32,W=32,X=64,Y=64,Z=128"

// Points per letter that is not at the intersection of
// a horizontal and vertical word within the crozzle.
NON_INTERSECTING_POINTS_PER_LETTER="A=0,B=0,C=0,D=0,E=0,F=0,G=0,H=0,I=0,J=0,K=0,L=0,M=0,N=0,O=0,P=0,Q=0,R=0,S=0,T=0,U=0,V=0,W=0,X=0,Y=0,Z=0"

Marking Test 3 – Word List File

AL,ALAN,ANGELA,BETTY,BEV,BEVERLY,BILL,BILLY,BRADLEY,BRAD,BRENDA,CATO,CENA,ED,EDWARD,FRED,FREDDY,GARY,GEORGE,GRACE,HARRY,JACK,JACKIE,JESS,JESSICA,JILL,JOHN,LARRY,LEO,LEW,LE,LEAH,LILY,MARK,MARY,MAT,MATTHEW,OSCAR,PAM,PAMELA,ROB,ROBERT,ROGER,ROD,RODDY,RONA,TOM,TOMMY,TRACE,TY,TYLER,VIC,VICTORIA,WENDY,WALTER
__________________________________________________________________________________


