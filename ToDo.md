# Frontstage Modernization – Proof of Concept

<<<<<<<< HEAD:readme.md
## Formål

Dette projekt er udviklet som et proof of concept med henblik på at undersøge, hvordan det eksisterende Frontstage-system kan moderniseres ved hjælp af moderne arkitekturprincipper såsom Domain-Driven Design (DDD), CQRS, Vertical Slice Architecture og API-First udvikling.
========
## Peter
- [x] Deployment diagram
- [x] Deployment unittests
- [x] Pipeline flow diagram
- [x] Fremtidig Kubernetes arkitektur
- [x] Debug container scan i pipeline
- [x] Debug tests, og integrer i pipeline

## Ea
- [x] Figure beskrivelser/numre
- [x] Fodnoter
- [ ] Integrere DCD i rapport
- [ ] Sekvensdiagram
- [x] Unittest: Login controller
- [x] Unittest: Login handler (behavior)
- [x] Unittest: Entity rule
>>>>>>>> master:ToDo.md

Løsningen demonstrerer håndtering af musik- og podcastkataloger samt integration mellem services via en Outbox-baseret integrationsmekanisme.

---

## Teknologistak

### Backend

- .NET 9
- ASP.NET Core
- Entity Framework Core
- MediatR
- FluentValidation
- ASP.NET Core Identity
- JWT Authentication
- PostgreSQL

### Frontend

- Blazor Server
- MudBlazor

### Arkitektur

- Domain-Driven Design (DDD)
- Vertical Slice Architecture
- CQRS
- Outbox Pattern
- Repository Pattern
- Unit of Work
- API-First Design

---

## Løsningsoversigt

| Projekt             | Beskrivelse                                                  |
| ------------------- | ------------------------------------------------------------ |
| Frontend            | Brugergrænseflade udviklet i Blazor                          |
| GatewayAPI          | API Gateway, authentication, authorization og JWT-udstedelse |
| MusicCatalogContext | Musikdomænet med tracks, releases, contributors m.m.         |
| PodcastCatalog      | Podcastdomænet med podcasts, seasons og episodes             |
| MusicQualityControl | Submission- og kvalitetskontrolflow                          |
| UserManagement      | Bruger- og rollehåndtering baseret på ASP.NET Identity       |
| OutboxWorker        | Behandler integrationshændelser fra Outbox-tabeller          |
| SharedKernel        | Fælles abstraktioner, interfaces og contracts                |
| Monitoring          | Monitoring og systemobservation                              |

---

## Startup-konfiguration

Følgende projekter skal som minimum startes:

- **Frontend**
- **GatewayAPI**

I Visual Studio: Solution → Configure Startup Projects → Multiple Startup Projects

### OutboxWorker (valgfri)

OutboxWorker håndterer asynkron kommunikation mellem bounded contexts. Den kan startes som et tredje projekt, men er ikke påkrævet for at systemet fungerer.

Hvis workeren ikke kører:

- Submissioner går ikke tabt
- Integrationshændelser gemmes i Outbox-tabellerne
- Hændelser behandles automatisk næste gang workeren startes

---

## Konfiguration

Projektet benytter User Secrets til følsom konfiguration såsom connection strings, JWT-indstillinger og interne API-nøgler.

Nødvendige secrets findes i rapportens bilag X.

Som minimum skal følgende projekter konfigureres:

- GatewayAPI
- OutboxWorker

---

## Database

Projektets primære database hostes bag et privat Tailscale-netværk.

### Mulighed 1 – Forbind til projektets database

1. Installer Tailscale
2. Tilslut projektets Tailscale-netværk
3. Anvend de connection strings der fremgår af bilag X

### Mulighed 2 – Lokal database

Mappen `database/` indeholder SQL-scripts som:

- Opretter databasestrukturen
- Opretter nødvendige roller og rettigheder
- Indsætter eksempeldata

Kør scriptet mod en lokal PostgreSQL-instans:

```bash
psql -U postgres -f database/init.sql
```

Dette gør det muligt at afvikle løsningen uden adgang til Tailscale-netværket.

---

## Login

Testbrugere og adgangsoplysninger fremgår af rapportens bilag X.

Roller anvendt i systemet:

- Admin
- Content Creator
- Quality Controller

---

## Kendte forhold

Projektet er udviklet som et proof of concept med fokus på arkitektur, integrationsmønstre og systemstruktur. Testdækningen er fokuseret på kritiske domæneregler og sikkerhedsflow. Visse dele af løsningen er bevidst simplificeret sammenlignet med et produktionsklart system.

---

## Dokumentation

Yderligere dokumentation findes i projektrapporten, herunder domænemodel, sekvensdiagrammer, arkitekturdiagrammer, deploymentdiagram, teststrategi samt bilag med konfiguration og adgangsoplysninger.
