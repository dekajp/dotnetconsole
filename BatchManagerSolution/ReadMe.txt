

============================================
Visual Studio Location of Solution file
============================================
C:\dotnetConsole\CLIAppManagerSolution

=================================
HOW TO EXECUTE THE APPLICATION
=================================
•	Thru CLI Program
	o	Provide parameters info thru comamnd line
	o	 CLIConsole.exe "CLIAppManager" "CLIAppManager.Drivers.SampleCLIProgramDriver" /SAMPLE_PARAM_1=HelloWorld
•	Thru Config File
	o	/CONFIG=TRUE will use Application Config file to populate all CLIParameters
	o	CLIConsole.exe "CLIAppManager" "CLIAppManager.Drivers.SampleCLIProgramDriver" /CONFIG=TRUE
•	Thru User Interaction , Override parameters
	o	CLIConsole.exe "CLIAppManager" "CLIAppManager.Drivers.SampleCLIProgramDriver" /P

