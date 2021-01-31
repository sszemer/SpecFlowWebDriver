@wikiSearch
Feature: wikiSearch
	In order to know what things are
	As an idiot
	I want to search for the definitions

@searchDefinitions
@web
Scenario Outline: Find definitions
	Given Wiki page is open
	When I search for a <definition>
	Then The definition of <definition> is displayed

	Examples: 
	| definition |
	| apple      |
	| pear       |

@checkCookies
@web
Scenario: Check Cookies
	Given Wiki page is open
	Then 'WMF-Last-Access' cookie value is '22-Jan-2021'

@checkLocalStorage
@web
Scenario: Check local storage
	Given Wiki page is open
	Then 'wmE-sS--sampleMultiplier' localStorage item value is '1'