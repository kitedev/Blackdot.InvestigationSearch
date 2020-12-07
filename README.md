# Blackdot.InvestigationSearch

# Overview:

This is a metasearch engine that aggregates search results scraped from Bing and Yahoo.

The solution consists of two projects:
1. A web application built using Angular 11 that allows a user to search for results given a search term.
2. A REStful Web API project built using ASP.Net Web API that will serve search results given a search term.


# Tools used

Visual Studio Code<br />
Visual Studio Community 2019


# Repository structure
Blackdot-InvestigationSearch-Client - The Angular metasearch engine web app
Blackdot.InvestigationSearch.Api - The API


# Setup

To run the web app, you must:<br />

1. Install npm
2. Install Visual Studio Code
3. Open Blackdot-InvestigationSearch-Client folder in Visual Studio Code
4. Open Terminal (in Visual Studio Code), change directory to
Blackdot-InvestigationSearch-Client and run 'npm install'.
(This should install everything you need. You may need to 
'npm install -g @angular/cli' to ensure you have Angular CLI)
5. Run 'ng serve' to host and run the app. Usually, this runs on http://localhost:4200/ 

To run the API, you must:<br />

1. Install Visual Studio 2019 (if you haven't already :/)
2. Open the solution file (Blackdot.InvestigationSearch.Api.sln) in Blackdot.InvestigationSearch.Api
3. Do a nuget package restore (dotnet restore)
4. Run. Hit f5.

(Note: the web app looks for the API on port 44375 (i.e. 'https://localhost:44375/api/search')
(i know it's hardcoded, and should be ready from a config)
This can be changed in launchSettings.json ("sslPort": 44375), in the API solution, if required.)

(Any issues, please feel free to contact me - kite.developer@live.com)

# Highlights

- Web App Solution Structure
- API Solution structure
- CQRS / Mediator pattern
- Validation
- SOLID principles
- Unit/Integration tests
- Swagger/OpenAPI Specification
- Server-side caching
- Thoughts & reflections

# Web App Solution Structure

In this Angular app, I have followed Component Driven Architecture by creating two components:
- investigation-search  - This is for the search input box and button.
- investigation-results - This is for displaying the search results in a table (with pagination and sorting)

The services folder contains the 'search' service (search.service.ts) which is responsble for making the
http GET request to the API.

The 'search-term' service (search-term.service.ts) is for raising a SearchTermSubmitted event.
The event is raised in the investigation-search component when clicking the search submit button,
and the investigation-results component subscribes to this event, which then calls the API to fetch the results.

Angular Material was used for the input, button, table, paginator and spinner.

# API Solution structure

I believe in writing modular and reusable code and this also applies to assembles/projects.
Below describes some of the projects and their reason behind them:

- Blackdot.InvestigationSearch.Api - 
The api layer should only contain code specific to this layer 
(no business/search logic), such as the controller, the Dto being responded and caching logic. 

- Blackdot.InvestigationSearch.Queries - 
Contains only the query events raised by the controller.

- Blackdot.InvestigationSearch.Queries.Handlers - 
Contains the query handler logic (GetSearchResultsQueryHandler), responsible for:
searchterm validation, running searches against each search engine (in parallel) and returning the results.

- Blackdot.InvestigationSearch.SearchEngines -
Contains the core logic for searching and scraping html from the search engines configured.


# CQRS / Mediator pattern

MediatR is a nice library for modularising and seperating concerns between what raises an event and your event handlers.
I have chosen to use this in the controller because this layer should only be concerned with 
logic concerning the web layer, caching, handling the response, http responses, etc

# Validation

I have used FluentValidation for the validation rules, which currently checks for null/empty search terms and a minimum character length.

# SOLID principles

Whilst structuring the classes, I’ve applied the SOLID principles.

I have applied Single Responsibility by ensuring classes are responsible for one purpose, such as:
CacheWrapper
SearchEngine
AngleSharpHtmlParser
QueryStringEncoder
etc

I believe I have demonstrated the use of the Open and Closed Principle because if another search engine is required,
then simply adding a new instance of SearchEngine to Startup.cs with it's specific configurations and adding a new implementations
for ISelector will suffice.

The use of dependency injection and interfaces also allows us to implement new implementations based on our needs.

I have applied the Interface Segregation Principle by ensuring a class does implement a method unnecessarily. 

Also, I have applied the Dependency Inversion Principle by using .Net Cores built in IoC container, I have ensured that functionality 
and implementations are injected into classes, enabling us to write modular code that is easily unit testable and mockable.


# Unit/Integration tests

I have written 34 tests. (I am proud to say that they are all passing! (Unless Yahoo has blocked you))

Some are integration tests, which are end to end tests, testing the full functionality.

The rest are unit tests test are for specific scenarios within a function. 

Regrettably, I didn’t have enough time to write tests for each and every class and flow 
(e.g. I haven’t tested for every possible exception) If I had more time, I would have done this.

I believe in 70-90% test coverage is good coverage, if possible.

I believe I have written enough tests to demonstrate my experience of writing them.


# Swagger/OpenAPI Specification

All APIs should have documentation. It’s useful for clients when consuming your API, as well as for the internal development team.

When running the solution, the documentation can be accessed via https://localhost:44375/swagger/index.html


# Server-side caching

This is simply a demonstration of how to use server side caching, 
which might be handy if we have many clients (investigators) searching for similar search terms in a short space of time.
I believe it is unlikely a search engine will be changing the results displayed within a short period of time (I could be wrong)

I have applied a sliding expiration for 10 minutes, and an absolute overarching expiration of 20 minutes. 
(I know it’s hardcoded, and really ought to be read from the web.config, please don’t judge me)


# Thoughts & reflections

If I had more time, I would have ensured a full test coverage.

I wonder if implementing the API with a GET was the right approach because of the query string and url encoding.
Perhaps a POST and passing the search term in the body would have been better , that way the client wouldn't have to worry about URL encoding.

I realise this search engine is basic and has no internal ordering logic. Whilst working on it, I was considering that
based on the search result URL, you could then make follow up subsequent requests to that page to find more meta data about the search term
such as the number of times the search term appears, the sentiment analysis, etc and using that for prioritising an order.

I was also temporarily banned from Yahoo due to the number of searches, I'd like to learn more about how to bypass this.
I read on the internet that you can trick search engines by setting http headers to mock being different clients, etc.

Interested to hear your thoughts and expertise on this.


If you read this far, well done. Thanks again for this opportunity.

