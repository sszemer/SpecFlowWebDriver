Feature: Android Chrome
	In order to know what things are
	As an idiot
	I want to google for the definitions

@android
@chrome
@mobile
Scenario: google things
	When I google for a <val>
	Then The google of <val> is displayed

	Examples:
		| val   |
		| thing |