﻿@NBPlookup
Feature: NBPlookup
	In order to go on vacation
	As a tourist
	I want to check if i can afford to buy local currency

@currencyLookup
Scenario: Lookup a currency rate
	Given NBP rest api is online
	When I lookup the currency for <currencyCode>
	Then I want to know if the rate is below <expectedRate>

	Examples: 
	| currencyCode | expectedRate |
	| THB          | 0.1300       |
	| USD          | 1            |