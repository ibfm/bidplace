# 🚀 BIDPlace — User Flows (Step-by-Step) & Service Responsibilities

> Business-friendly overview. Each step shows **which services** are involved.

---

## 👤 A) Participant (Web + Mobile)

**Goal:** register/sign in, browse catalog (Web & Mobile), request participation, pass KYC, and **bid only in the Mobile app**.

### 🔑 Flow A1 — Sign in (Web or Mobile)
1. Click **Sign in with Google/Apple**.  
   **Services:** 🔐 **Identity.API** (OAuth/OIDC), 🚪 **YARP Gateway** (single entrypoint for Mobile).
2. Identity issues a **secure token**; user becomes a **Participant**.  
   **Services:** 🔐 **Identity.API**

### 🗂️ Flow A2 — Browse auctions (Web & Mobile)
3. View **public catalog** and open an **auction detail**.  
   **Services:** 💻 **WebApp** / 📱 **ClientApp**, 🧭 **Catalog.API**

### ✋ Flow A3 — Request to participate (Web & Mobile)
4. Tap **“Request to participate”** on an auction page.  
   **Services:** 🧑‍🤝‍🧑 **Person.API → Participation** (creates request linked to `auctionId`)

### 🪪 Flow A4 — Identity verification (KYC)  
*(Requested on Web or Mobile, completed on the **phone** for best UX)*
5. Complete **IDWall** KYC on the phone (document + selfie).  
   **Services:** 🧑‍🤝‍🧑 **Person.API → Participants & Verification**, ⚙️ **Person.Verifier (Worker)**, ☁️ **IDWall**, ✉️ **RabbitMQ** (queue), 🗄️ **Blob Storage** (docs)
6. After KYC approval, request becomes **IdentityVerified**.  
   **Services:** 🧑‍🤝‍🧑 **Person.API**, ⚡ **Redis** (short cache)

### 🏷️ Flow A5 — Organizer review (Web)
7. The auction’s **Organization** reviews and **Approves/Rejects**.  
   **Services:** 💻 **WebApp (Org)**, 🧑‍🤝‍🧑 **Person.API → Participation**, 🧭 **Catalog.API** (ownership context)
8. If **Approved**, the user is **Enabled** for that auction.  
   **Services:** 🧑‍🤝‍🧑 **Person.API** (event for ⚡ bidding)

### ⚡ Flow A6 — Live bidding (**Mobile only**)
9. On Mobile, enter the **live room** and place bids in real time.  
   **Services:** 📱 **ClientApp**, 🔔 **Bids.API** (SignalR/WebSockets), 🧑‍🤝‍🧑 **Person.API** (eligibility), ⚡ **Redis** (pub/sub), 📣 **Notifications (Firebase)**

---

## 🏢 B) Organization (Supplier: Auctioneer, Bank, Insurer) — Web

**Goal:** apply to the platform, get approved, create auctions/lots, approve participants, monitor live.

### 📝 Flow B1 — Apply as Supplier
1. Click **“Suppliers”** → registration form → submit company data & documents.  
   **Services:** 💻 **WebApp**, 🧑‍🤝‍🧑 **Person.API → Organizations**, 🗄️ **Blob Storage**

### ✅ Flow B2 — Admin approval
2. **Admin** reviews and **Enables/Disables** the organization.  
   **Services:** 💻 **WebApp (Admin)**, 🧑‍🤝‍🧑 **Person.API → Organizations**

### 🧭 Flow B3 — Create & manage auctions
3. Create **Auctions** and **Lots**.  
   **Services:** 💻 **WebApp (Org)**, 🧭 **Catalog.API**

### 👥 Flow B4 — Approve participation requests
4. See **Participation Requests** per auction and **Approve/Reject**.  
   **Services:** 💻 **WebApp (Org)**, 🧑‍🤝‍🧑 **Person.API → Participation**, 🧭 **Catalog.API** (owner check)

### 📊 Flow B5 — Monitor live
5. Monitor the live room and outcomes.  
   **Services:** 💻 **WebApp (Org)**, 🔔 **Bids.API** (read), 🧭 **Catalog.API** (context)

---

## ⚖️ C) Magistrate — Web (Read-only)

**Goal:** register as magistrate, get approved by Admin, **view judicial auctions** (future: cancel when applicable).

### 📝 Flow C1 — Register
1. Click **“Magistrates”** → registration form → submit CPF + official judge ID and docs.  
   **Services:** 💻 **WebApp**, 🧑‍🤝‍🧑 **Person.API → Magistrates**, 🗄️ **Blob Storage**

### ✅ Flow C2 — Admin approval
2. **Admin** reviews and **Enables** magistrate access (read-only).  
   **Services:** 💻 **WebApp (Admin)**, 🧑‍🤝‍🧑 **Person.API → Magistrates**

### 👀 Flow C3 — Read-only access
3. View **judicial auctions** relevant to their jurisdiction.  
   **Services:** 💻 **WebApp**, 🧭 **Catalog.API**

---

## 🛡️ D) Admin — Web

**Goal:** keep the platform safe and compliant. **Admin user is pre-seeded via migrations** (exists from day one).

### 🔐 Flow D1 — Sign in (with MFA)
1. Admin signs in; **MFA** enforced.  
   **Services:** 🔐 **Identity.API** (pre-seeded Admin role/user), 💻 **WebApp (Admin)**

### 🏢 Flow D2 — Review & enable Suppliers
2. List **Organizations** and **Approve/Disable**.  
   **Services:** 💻 **WebApp (Admin)**, 🧑‍🤝‍🧑 **Person.API → Organizations**

### ⚖️ Flow D3 — Review Magistrates
3. List **Magistrates** and **Approve/Disable**.  
   **Services:** 💻 **WebApp (Admin)**, 🧑‍🤝‍🧑 **Person.API → Magistrates**

### 🧾 Flow D4 — Oversight & audit
4. Check **audit logs & reports** for onboarding and participation decisions.  
   **Services:** 💻 **WebApp (Admin)**, 🧑‍🤝‍🧑 **Person.API** (audit trail), 📈 **OpenTelemetry**

> **Database Seed (migrations):** create **Admin role** + **Admin user** (email, hashed password, MFA required) so the platform is manageable immediately after deployment.

---

# 🧭 Service Summary & Responsibilities

- 🚪 **YARP Gateway (required)**  
  One front door for **Mobile**: centralized auth enforcement, rate limiting, routing, and request shaping. Simplifies OAuth/OIDC callbacks and reduces CORS issues.

- 🔐 **Identity.API (IdP)**  
  **AuthN/AuthZ**: username/password (Admin/Org/Mag) and **social (Google/Apple)** for Participants.  
  Issues **JWT/refresh tokens**; exposes **JWKS**.  
  **Admin seeding via migrations** (role/user + MFA policy).

- 🧑‍🤝‍🧑 **Person.API (Business identity & eligibility)**  
  **Organizations** (registration, docs, **enable/disable**),  
  **Magistrates** (registration + approval; read-only checks via claims),  
  **Participants** (profile, **device binding** via secure device checks, **KYC** with IDWall),  
  **Participation** (per-auction workflow: `Created → IdentityVerified → Enabled | Rejected`).  
  ⚙️ **Person.Verifier (Worker)** handles KYC webhooks/queues and emits domain events.

- 🧭 **Catalog.API (What is being auctioned)**  
  **Auctions** (events) & **Lots** (items). Each auction belongs to an **Organization**.

- 🔔 **Bids.API (The live room)**  
  Real-time bidding (SignalR/WebSockets), **extension** rules.  
  Validates **eligibility** with Person (cached via Redis).  
  Emits `AuctionClosed/Won` events.

- 💻 **WebApp (Browser)**  
  **Public** catalog & auction detail.  
  **Admin portal**: approve/disable Suppliers & Magistrates; audit views.  
  **Organization portal**: create/manage Auctions & Lots; approve participation.

- 📱 **ClientApp (Mobile)**  
  **Participant app**: sign in (Google/Apple), request participation, complete KYC, **bid live**.

---

# 🧩 Infrastructure (Aspire)

- 🗄️ **PostgreSQL** — persistent data (schemas per domain: identity, person, catalog, bids)  
- ⚡ **Redis** — cache (eligibility) & pub/sub for real time  
- ✉️ **RabbitMQ** — orchestration/queues (KYC & domain events)  
- 🗂️ **Blob Storage** — documents (org/magistrate), images  
- 📣 **Notifications (Firebase)** — push to Mobile  
- 📈 **OpenTelemetry** — logs/metrics/traces for observability

**Traffic path:**  
- **Web:** 🌐 Browser → 💻 WebApp → (🔐 Identity for login) → 🧭 Catalog / 🧑‍🤝‍🧑 Person  
- **Mobile:** 📱 ClientApp → 🚪 **YARP Gateway** → (🔐 Identity for login) → 🧑‍🤝‍🧑 Person / 🧭 Catalog / 🔔 Bids
