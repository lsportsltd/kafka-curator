# kafka-curator

## Description 

Manage environments (DEV\QA\Prod) Kafka topics:
* Adding topics
* Alter topics to suite configuration 
* Delete unexlude topics

Configuration File per Environments:
* DEV - ./src/Logic/KafkaCurator/topicsettings.dev.json
* QA - ./src/Logic/KafkaCurator/topicsettings.qa.json
* PROD - ./src/Logic/KafkaCurator/topicsettings.prod.json

## CI\CD
Pipeline is triggered in Azure Devops ['Jobs'](https://dev.azure.com/lsportsltd/Jobs/_build?definitionId=579&_a=summary)  project


## PR Code review & pipeline approvals 
  Approvers 
  * Team leaders 
  * Architects 
  * DevOps team

