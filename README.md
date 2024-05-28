## Overview

1. [Purpose of this app](#1-purpose-of-this-app)
2. [Architecture](#2-architecture)
   1. [Use Cases](#i-use-cases)
   2. [Activities](#ii-activities)
   3. [Entities](#iii-entities)
   4. [Architecture Type](#iv-architecture-type)
   5. [Layers Interaction](#v-layers-interaction)
   6. [Classes](#vi-classes)
3. [Videos](#3-videos)
4. [Team Tasks](#4-team-tasks)

## 1. Purpose of this app

- To implement a bookstore-like platform with the basic functions of a store.
- To design an architecture based on the analysis of the app specification without unnecessary overheads.
- To achieve complexity based on holistic simplicity: loose coupling, strong cohesion, and modularity.

## 2. Architecture

### I. Use Cases

- We need to accommodate three separate use cases. Each user type will have its own UI.

![use_case_diagram.png](images/use_case_diagram.png)

### II. Activities

- Based on these use cases, we must define each flow that a user type can accomplish.

| Admin Activity Diagram                                           | Client Activity Diagram                                            | Provider Activity Diagram                                              |
|------------------------------------------------------------------|--------------------------------------------------------------------|------------------------------------------------------------------------|
| ![admin_activity_diagram.png](images/admin_activity_diagram.png) | ![client_activity_diagram.png](images/client_activity_diagram.png) | ![provider_activity_diagram.png](images/provider_activity_diagram.png) |

### III. Entities

- As we moved further, we identified the required entities as follows:

![entity_diagram.png](images/entity_diagram.png)

### IV. Architecture Type

- Based on previous analysis, we identified that a layered pattern will fit our application specifics.
- About the layers:
    - Presentation is split into three separate parts (see the use cases).
    - Business includes a series of services for authentication, privacy, etc. Additionally, it defines a series of BTOs
      meant for presentation-related logic.
    - Persistence contains a series of repositories that encapsulate CRUD operations for each entity. Furthermore, it
      defines a series of DTOs meant for business/presentation-related data flow.
    - Commons contain various general-purpose utilities like a scoped logger and a generic implementation of a result
      pattern.
    - Integration includes unit tests of the persistence and business layers as well as database seeding.

![layered_architecture.png](images/layered_architecture.png)

### V. Layers Interaction

- Now that we have split our app into distinct functionalities, it's time to define the logical interaction between
  layers.

| Admin Sequence Diagram                                           | Client Sequence Diagram                                            | Provider Sequence Diagram                                              |
|------------------------------------------------------------------|--------------------------------------------------------------------|------------------------------------------------------------------------|
| ![admin_sequence_diagram.png](images/admin_sequence_diagram.png) | ![client_sequence_diagram.png](images/client_sequence_diagram.png) | ![provider_sequence_diagram.png](images/provider_sequence_diagram.png) |

### VI. Classes

- One of the primary design aspects was to ensure well-defined boundaries. The following class diagram supports that:

![class_diagram.png](images/class_diagram.png)

# 3. Videos
