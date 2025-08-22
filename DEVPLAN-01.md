# 🚀 BIDPlace — Development Plan & Roadmap

This plan starts from what we have today and lands the MVP for the flows below, with phased delivery, demo gates, and acceptance criteria.

---

## 📌 Current State (today)

- 🔐 **Identity.API** present (from eShop base); needs OIDC providers (Google/Apple) + **Admin seeding via migrations**.
- 🌐 **WebApp** present (admin/public); will host Supplier & Magistrate registration screens.
- 📱 **ClientApp (MAUI)** present; will be the **only** place where bidding happens.
- 🧭 **Catalog/Lots** code exists to be evolved into **Catalog.API** (Auctions + Lots).
- 🚪 **YARP Gateway** package present; must be **enabled** as the **required** mobile/API entrypoint.
- 🧑‍🤝‍🧑 **Person.API** (new): to hold Organizations, Magistrates, Participants (KYC + device), Participation.
- 🔔 **Bids.API** (new): real-time bidding, rules, SignalR.
- 🧱 Infra libs: PostgreSQL, RabbitMQ, Redis already referenced in AppHost.

---

## 🎯 MVP Outcome (what “done” means)

- 🧑‍💼 **Admin** can sign in (MFA), see **two lists** (Organizations & Magistrates), **approve/disable**.
- 🏢 **Organization** can register on Web, be approved, create **Auctions & Lots**, and **approve participants**.
- ⚖️ **Magistrate** can register on Web, be approved, and **view judicial auctions** (read-only).
- 👤 **Participant** can sign in on **Web & Mobile**, **request participation** from either, complete **KYC on phone**, get **approved**, and **bid live** **(Mobile only)**.
- 🔐 Identity is unified via **Identity.API**, with **YARP** as the **gateway** for Mobile.
- 📈 Basic observability, audit trails on approvals and participation, rate limits, and secure storage for docs (Blob).

---

## 🗺️ Roadmap (Sprints & Milestones)

> Sprints are 1–2 weeks each. Each sprint ends with a **demo gate** (✓ criteria).  
> Items marked **(R)** are required pre-prod hardening.

### 🧰 Sprint 0 — Foundations & Seeding
**Goals**
- Enable **YARP** as mobile/API gateway.
- Configure **Identity.API** with Google/Apple (OIDC/PKCE).
- **Seed Admin** (role + user + MFA) via migrations.
- Set up Aspire local env (PostgreSQL, Redis, RabbitMQ), secrets, CI checks.

**Deliverables**
- YARP routes: `/identity`, `/person`, `/catalog`, `/bids` for Mobile.
- `Identity.API`: Google/Apple clients, JWKS exposed, refresh rotation.
- Migration with `Admin` role/user & MFA policy; default password rules.
- AppHost wiring + health endpoints.

**Demo Gate (✓)**
- Admin logs in with MFA through WebApp.
- Mobile hits YARP and completes OIDC on a test screen (no business calls yet).

---

### 🧑‍🤝‍🧑 Sprint 1 — Person.API (Organizations & Magistrates) + Web screens
**Goals**
- Scaffold **Person.API** with modules: `Organizations`, `Magistrates`, `Audit`.
- WebApp screens: **Suppliers** (register) and **Magistrates** (register).
- Admin pages: **two lists** (Orgs & Magistrates) with **Approve/Disable**.

**Deliverables**
- Endpoints:
  - `POST /api/organizations` (with docs upload)  
  - `POST /api/magistrates`  
  - `GET /api/admin/organizations`, `POST /api/admin/organizations/{id}/enable|disable`  
  - `GET /api/admin/magistrates`, `POST /api/admin/magistrates/{id}/enable|disable`
- Blob upload + metadata, audit log entries for decisions.

**Demo Gate (✓)**
- Org & Magistrate can submit via Web; Admin can approve/disable and see audit entries.

---

### 🧭 Sprint 2 — Catalog.API (Auctions & Lots) + Org Portal
**Goals**
- Evolve existing code into **Catalog.API** with `Auctions` + `Lots` (owner = `organizationId`).
- WebApp (Org): **Create/Manage** Auctions & Lots.

**Deliverables**
- Endpoints:
  - `POST/GET /api/auctions` (owned by organization)  
  - `POST/GET /api/auctions/{id}/lots`
- RBAC checks (Org owns what it edits).

**Demo Gate (✓)**
- Approved Org creates an auction with lots and sees it in the public catalog.

---

### 👤 Sprint 3 — Participants & Participation (Web & Mobile request)
**Goals**
- Person.API: `Participants` (profile, device binding stub), `Participation` (join request).
- Web & Mobile: **Request to participate** from auction detail.

**Deliverables**
- Endpoints:
  - `POST /api/participation/requests` `{ auctionId }`  
  - `GET /api/participation/eligibility?auctionId=...`  
  - `POST /api/participants/devices/register` (publicKey, attestation placeholder)
- WebApp (Org): list **Participation Requests** per auction; **Approve/Reject** actions.

**Demo Gate (✓)**
- Participant (Web or Mobile) requests to join; Org sees the request and can approve/reject (*without KYC yet*).

---

### 🪪 Sprint 4 — KYC Integration (IDWall) & Worker
**Goals**
- Integrate **IDWall** end-to-end (start + webhook).
- Build **Person.Verifier** worker (RabbitMQ) with retries/backoff.
- Switch device binding to **secure attestation** (Android/iOS), no IMEI.

**Deliverables**
- Endpoints:
  - `POST /api/participants/kyc/start` → returns mobile KYC link/params  
  - `POST /api/kyc/idwall/callback` (webhook)  
  - Worker: updates `participant_verifications` → `Approved/Rejected`; transitions request to `IdentityVerified`
- Doc retention policy (Blob), audit enrichment.

**Demo Gate (✓)**
- New participant completes KYC on phone; request flips to **IdentityVerified** automatically.

---

### ⚡ Sprint 5 — Bids.API (live) & Eligibility
**Goals**
- Build **Bids.API**: rooms, place bid, timer + extension rules (MVP).
- Eligibility: Bids validates “**Enabled for auction**” via Person (Redis cached).
- Push notifications (Firebase) on auction events.

**Deliverables**
- SignalR hub `/bids/hub` (join/leave/bid), Redis pub/sub, basic scoreboard.
- Extension rule (e.g., add 10–30s on near-end bid).
- Events: `AuctionClosed/Won`.

**Demo Gate (✓)**
- Mobile joins a live auction room, places bids, sees timer extend, and auction close.

---

### 🛡️ Sprint 6 — Hardening, Observability & Launch Readiness (R)
**Goals**
- Security reviews (rate limiting, idempotency, JWT scopes, CORS), perf testing on Bids.
- OpenTelemetry dashboards (latency, room QPS, error rates, KYC queue lag).
- Docs/runbooks, backup/restore, on-call rotation, env promotion checklist.

**Deliverables**
- k6/Gatling load test artifacts for Bids.
- Alerting thresholds (Redis pressure, Rabbit lag, error spikes).
- Runbooks: incident, rollback, key rotation, IDWall outage fallback.

**Demo Gate (✓)**
- 60+ concurrent bidders per room in staging (example target), stable p95 latency; audits exportable.

---

## ✅ Acceptance Criteria (MVP recap)

- **Admin (seeded)** can approve/disable **Orgs** and **Magistrates**.
- **Orgs** can create **Auctions & Lots** and approve **Participants**.
- **Participants** can sign in (Web/Mobile), request participation, complete **KYC on phone**, and **bid live (Mobile)**.
- **YARP** is enforced as gateway for Mobile; Identity centralizes tokens.
- **Audit trails** exist for onboarding & participation; basic dashboards live.

---

## 🧭 Services & Responsibilities (final)

- 🚪 **YARP Gateway (required)**: single entrypoint for **Mobile**; auth enforcement, rate limiting, routing.
- 🔐 **Identity.API (IdP)**: AuthN/AuthZ; Google/Apple; **Admin seeded via migrations**; JWT/refresh; JWKS.
- 🧑‍🤝‍🧑 **Person.API**: Organizations, Magistrates, Participants (device + **KYC**), Participation workflow; **Person.Verifier** worker (IDWall).
- 🧭 **Catalog.API**: Auctions & Lots; ownership = Organization.
- 🔔 **Bids.API**: real-time engine (SignalR), rules, eligibility (Person + Redis), `AuctionClosed/Won`.
- 💻 **WebApp**: Public catalog; Admin lists & approvals; Org auctions & participation queue.
- 📱 **ClientApp (MAUI)**: Participant app (sign in, request participation, **bid**).

---

## ⚠️ Key Risks & Mitigations

- **OIDC across Web & Mobile**: use YARP for callback uniformity; enforce PKCE; test Apple/Google in staging.
- **KYC vendor dependency**: implement robust webhook validation, retries, and a manual failover path.
- **Device “IMEI”**: **do not store**; use **public key + platform attestation**.
- **Bids load**: Redis capacity planning, backpressure, rate limits per user/auction.

---

## 🧪 Definition of Done (per feature)

- Unit/integration tests (≥70% on module), authZ tests for roles.
- OpenAPI updated; postman collection updated.
- Metrics/logs/traces instrumented; dashboards refreshed.
- Docs in `/docs` updated; runbooks adjusted.

---

## 📦 Out of Scope (post-MVP backlog)

- 💳 Billing/Subscriptions for Organizations (C1) and commission settlement.
- 🧠 Recommendations/AI summaries.
- 🧩 Magistrate **cancel** authority (requires extra audit & flows).

