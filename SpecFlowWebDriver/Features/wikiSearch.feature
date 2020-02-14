@wikiSearch
Feature: wikiSearch
	In order to know what things are
	As an idiot
	I want to search for the definitions

@searchDefinitions
Scenario: Find definitions
	Given Wiki page is open
	When I search for a <definition>
	Then The definition of <definition> is displayed

	Examples: 
	| definition |
	| apple      |
	| pear       |