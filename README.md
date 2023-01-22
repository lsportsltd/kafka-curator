# kafka-curator

## Description 

Manage environments (DEV\QA\Prod) Kafka topics:
* Adding topics
* Alter topics to suite configuration 
* Delete unexlude topics

Configuration File per Environments:
* DEV - ./src/Logic/LSports.Kafka.Curator/topicsettings.dev.json
* QA - ./src/Logic/LSports.Kafka.Curator/topicsettings.qa.json
* PROD - ./src/Logic/LSports.Kafka.Curator/topicsettings.prod.json

To view a list of all available topic configurations, press [here](https://github.com/nizanrosh/kafka-curator#topic-configuration).

## CI\CD
Pipeline is triggered in Azure Devops ['Jobs'](https://dev.azure.com/lsportsltd/Jobs/_build?definitionId=579&_a=summary)  project

## PR Code review & pipeline approvals 
  Approvers 
  * Team leaders 
  * Architects 
  * DevOps team

