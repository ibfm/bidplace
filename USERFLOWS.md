# BIDPlace — User Flows (Step-by-Step) and Service Responsibilities

> Non-technical, business-friendly flows. Each step lists **which services** are involved.

---

## A) Participant (Web + Mobile)

**Goal:** register/sign in, browse catalog (Web & Mobile), request participation, pass KYC, and **bid only in the Mobile app**.

### Flow A1 — Sign in (Web or Mobile)
1. User clicks **Sign in with Google/Apple**.  
   *Services:* **Identity.API** (OAuth/OIDC), **YARP Gateway** (Mobile single entrypoint).

2. Identity issues a **secure token**; the user becomes a **Participant**.  
   *Services:* **Identity.API**.

### Flow A2 — Browse auctions (Web and Mobile)
3. User views **public catalog** and opens an **auction detail**.  
   *Services:* **WebApp / ClientApp**, **Catalog.API**.

### Flow A3 — Request to participate (Web and Mobile)
4. User taps **“Request to participate”** in a specific auction.  
   *Services:* **Person.API → Participation** (creates request linked to `auctionId`).

### Flow A4 — Identity verification (KYC) (Web triggers, completed on **Mobile**)
5. Platform prompts user to **complete KYC on the phone** (document + selfie).  
   *Services:* **Person.API → Participants & Verification**, **Person.Verifier (Worker)**, **IDWall**, **RabbitMQ** (queue), **Blob** (docs).

6. After KYC approval, request status becomes **IdentityVerified**.  
   *Services:* **Person.API**, **Redis** (short-term cache).

### Flow A5 — Organizer review (Web)
7. The auction’s **Organization** reviews and **Approves/Rejects** the request.  
   *Services:* **WebApp (Org)**, **Person.API → Participation**, **Catalog.API** (ownership context).

8. If **Approved**, the user is **Enabled** for that auction.  
   *Services:* **Person.API**, event published for **Bids.API**.

### Flow A6 — Live bidding (**Mobile only**)
9. On Mobile, user enters the **live room** and places bids in real time.  
   *Services:* **ClientApp (Mobile)**, **Bids.API** (real-time engine), **Person.API** (eligibility check), **Redis** (pub/sub), **Notifications (Firebase)**.

---

## B) Organization (Supplier: Auctioneer, Bank, Insurer) — Web only

**Goal:** apply to the platform, get approved, create auctions/lots, approve participants, monitor live.

### Flow B1 — Apply as Supplier (Web)
1. Click **“Suppliers”** button → registration screen → submit company data & documents.  
   *Services:* **WebApp**, **Person.API → Organizations**, **Blob** (docs).

### Flow B2 — Admin approval (Web)
2. **Admin** reviews and **Enables/Disables** the organization.  
   *Services:* **WebApp (Admin)**, **Person.API → Organizations**.

### Flow B3 — Create & manage auctions (Web)
3. Approved organization creates **Auction** and **Lots**.  
   *Services:* **WebApp (Org)**, **Catalog.API**.

### Flow B4 — Approve participation requests (Web)
4. Organization sees **“Participation Requests”** for each auction and **Approves/Rejects**.  
   *Services:* **WebApp (Org)**, **Person.API → Participation**, **Catalog.API** (owner check).

### Flow B5 — Monitor live (Web)
5. Organization monitors the live room and outcomes.  
   *Services:* **WebApp (Org)**, **Bids.API** (read), **Catalog.API** (context).

---

## C) Magistrate — Web only (Read-only)

**Goal:** register as magistrate, get approved by Admin, **view judicial auctions** (future: cancel when applicable).

### Flow C1 — Register (Web)
1. Click **“Magistrates”** button → registration screen → submit CPF + official judge ID and docs.  
   *Services:* **WebApp**, **Person.API → Magistrates**, **Blob** (docs).

### Flow C2 — Admin approval (Web)
2. **Admin** reviews and **Enables** magistrate access (read-only).  
   *Services:* **WebApp (Admin)**, **Person.API → Magistrates**.

### Flow C3 — Read-only access (Web)
3. Magistrate can **view judicial auctions** relevant to their jurisdiction.  
   *Services:* **WebApp**, **Catalog.API**.

---

## D) Admin — Web only

**Goal:** keep the platform safe and compliant. **Admin user is pre-seeded via migrations** (exists from day one).

### Flow D1 — Sign in (with MFA)
1. Admin signs in; MFA is enforced.  
   *Services:* **Identity.API** (pre-seeded Admin user/role), **WebApp (Admin)**.

### Flow D2 — Review & enable Suppliers
2. Admin lists **Organizations** and **Approves/Disables**.  
   *Services:* **WebApp (Admin)**, **Person.API → Organizations**.

### Flow D3 — Review Magistrates
3. Admin lists **Magistrates** and **Approves/Disables**.  
   *Services:* **WebApp (Admin)**, **Person.API → Magistrates**.

### Flow D4 — Oversight & audit
4. Admin checks **audit logs & reports** for onboarding and participation decisions.  
   *Services:* **WebApp (Admin)**, **Person.API** (audit trail), **OpenTelemetry** (logs/metrics).

> **Database Seed (migrations):** create Admin role + Admin user (email, hashed password, MFA required) so the platform is manageable immediately after deployment.

---

# Services (short description & responsibility)

- **YARP Gateway (required)**
  - Single entrypoint for **Mobile**; central auth enforcement, routing, rate limiting, request shaping.
  - Simplifies OAuth/OIDC callbacks for the ClientApp and reduces CORS issues.

- **Identity.API (IdP)**
  - **Authentication/Authorization**: username/password (Admin/Org/Mag) and **social (Google/Apple)** for Participants.
  - Issues **JWT/refresh tokens** and exposes **JWKS**.
  - **Admin seeding via migrations** (Admin role/user + MFA policy).

- **Person.API (Business identity & eligibility)**
  - **Organizations**: registration, documents, **enable/disable** (exposed to Admin).
  - **Magistrates**: registration + approval; read-only access later is checked via claims.
  - **Participants**: profile, **device binding** (secure device check), **KYC** with IDWall.
  - **Participation**: per-auction request workflow  
    `Created → IdentityVerified → Enabled | Rejected`.
  - **Person.Verifier (Worker)**: processes KYC webhooks/queues and emits domain events.

- **Catalog.API (What is being auctioned)**
  - **Auctions** (events) and **Lots** (items); an Auction has an owning **Organization**.

- **Bids.API (The live room)**
  - Real-time bidding (SignalR/WebSockets), **extension** rules.
  - Validates **eligibility** with Person (cached via Redis).
  - Emits `AuctionClosed/Won` events.

- **WebApp (Browser)**
  - **Public** catalog & auction detail.
  - **Admin portal**: approve/disable Suppliers & Magistrates; audit views.
  - **Organization portal**: create/manage Auctions & Lots; approve participation.

- **ClientApp (Mobile)**
  - **Participant app**: sign in (Google/Apple), request participation, complete KYC, **bid live**.

---

# Infrastructure (Aspire)

- **PostgreSQL** — persistent data (schemas per domain: identity, person, catalog, bids).
- **Redis** — cache (eligibility), pub/sub for real-time.
- **RabbitMQ** — orchestration/queues (KYC & events).
- **Blob Storage** — documents (org/magistrate), images.
- **Notifications (Firebase)** — push to ClientApp.
- **OpenTelemetry** — logs/metrics/traces for observability.

**Traffic path:**
- **Web**: Browser → WebApp → (Identity for login) → Catalog/Person APIs.
- **Mobile**: ClientApp → **YARP Gateway** → (Identity for login) → Person/Catalog/Bids.