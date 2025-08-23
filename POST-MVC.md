# 🚀 BIDPlace — Post-MVP Feature Map

> A curated list of features to build **after the MVP**. Grouped by domain with brief rationale and dependencies.

---

## 👤 Participant Experience

- 🔁 **Document Reuse Library**
  - Reuse previously uploaded docs (e.g., address proof, ID photo) across auctions.
  - Policies: validity window per docType, org-scoped reuse vs cross-org, forced re-review.
  - Dep: Participant document vault, verification records, hashing.

- 📱 **Deep-link & Handoff**
  - Seamless handoff from Web to Mobile (QR/deep link) for KYC and step completion.

- 📨 **Multi-channel Notifications**
  - Push/SMS/Email for queue updates (approved, missing docs, auction starting soon).

- 🌐 **Localization & Timezones**
  - PT-BR + EN UI, localized date/number formats, automatic timezone handling.

---

## 🏢 Organization Tools (Suppliers)

- 🧠 **Configurable Eligibility Engine (per auction)**
  - KYC **mandatory & first**; add ordered steps afterwards: Email OTP, SMS OTP, DocSign Upload, File Upload (deposit/address), custom questionnaires.
  - Step flags: `required`, `requiresReview`, `validityDays`, `reuse policy`.
  - Dep: Person.API workflow engine; Catalog ↔ flow snapshot link.

- 🗂️ **Eligibility Board (Kanban)**
  - Columns = steps (in order); Cards = participants.
  - Quick actions: approve/reject (with notes), filter/search, SLA timers.
  - Dep: step instances, review queue, audit log.

- 🖊️ **Native E-signature Integration**
  - DocuSign/Adobe Sign connectors, template versions, signature evidence storage.

- 🔗 **Partner Webhooks (Outbound)**
  - Notify org ERPs/CRMs on `ParticipationEnabled`, `AuctionClosed`, `WinnerSettled`.

- 🧾 **Invoicing & Statements (Pre-Billing)**
  - Monthly statements for orgs: participation volumes, winners, pending settlements.

---

## ⚖️ Magistrate & Compliance

- ⛔ **Judicial Auction Cancelation**
  - Read-only today; add authorized cancel with reason codes, hard audit trail.
  - Dep: role-based claims, irreversible event record, notification fan-out.

- 📜 **Regulatory Exports & Audit**
  - Exportable logs (CSV/JSON), immutable hashes, retention policies, evidence bundles.

- 🧩 **Policy Packs**
  - CNJ-aligned templates (terms, disclosures), mandatory notices, rate limits.

---

## 🔐 Identity, Security & Risk

- 🔐 **MFA Across Roles**
  - TOTP/WebAuthn options; adaptive enforcement (high-risk actions).

- 📱 **Device Binding (Attestation)**
  - Android/iOS attestation keys, rotation, jailbreak/root detection signals.

- 🕵️ **Risk Scoring / AML Flags**
  - Signals from velocity, device reputation, KYC inconsistencies; manual review queue.

- 🔏 **Secrets & Key Rotation Playbooks**
  - Automated rotation for JWT signing keys, provider secrets; break-glass runbooks.

---

## 🔔 Bidding Engine Enhancements

- ⏱️ **Advanced Extension Rules**
  - Configurable thresholds per lot, max total extensions, anti-sniping analytics.

- 👥 **Spectator/Readonly Feeds**
  - High-fanout read replicas/streams for public viewing without auth.

- 🧪 **Load-tested Rooms**
  - Targeted concurrency (e.g., 1k bidders / room); autoscale; backpressure strategies.

---

## 🧭 Catalog, Search & Discovery

- 🔎 **Full-text & Faceted Search**
  - Brand/model/price/location filters; synonyms; highlights.

- 📍 **Geo-filters & Maps**
  - Filter by state/UF/radius; visual map of lots/pickup points.

- 🖼️ **Media Pipeline**
  - Image optimization, thumbnails, safe-content checks, lazy loading.

---

## 💳 Payments, Billing & Settlements

- 💼 **Billing Service**
  - SaaS (C1) invoicing + 1% commission; PIX/credit card; dunning flows.
  - Events: consume `AuctionClosed/Won`.

- 💰 **Deposit (Caução) Providers**
  - Optional switch from “proof upload” to **escrow/PIX with webhook confirmation**.
  - Reconciliation dashboard; partial refunds; dispute escalation.

---

## 🔌 Integrations & Webhooks

- 🔂 **Inbound Webhooks**
  - KYC/e-sign/payment callbacks with HMAC signatures, retries & DLQ.

- 📨 **Outbound Webhooks**
  - Subscription UI for partners; per-event delivery status, replay, idempotency keys.

- 🧰 **Provider Abstraction**
  - Pluggable adapters for Email/SMS, KYC, e-sign, payments (feature flags per org).

---

## 📈 Observability, Reliability & Ops

- 📊 **Dashboards**
  - Bids p95 latency, room QPS, Redis pressure, KYC queue lag, OTP success rate.

- 🌩️ **SLOs & Incident Runbooks**
  - Error budgets; on-call procedures; circuit breakers; graceful degradation.

- 🌍 **Multi-region / DR**
  - Read replicas, cross-region failover plans; auction room pinning strategies.

- 🧪 **Perf & Chaos Testing**
  - k6/Gatling suites, chaos experiments for Redis/Rabbit/DB.

---

## 🧱 Data & Governance

- 🗄️ **Data Warehouse Feeds**
  - Nightly exports to DW/Lake (participants, auctions, bids, KYC outcomes).

- 🔐 **LGPD Tooling**
  - Subject access requests, data minimization, retention schedulers.

- 🧮 **Pricing & Revenue Analytics**
  - Cohorts by org, ARPA, auction GMV, conversion from request→enabled→bid→win.

---

## 🧑‍💻 Developer Experience

- 🧪 **Contract Tests & Sandboxes**
  - Provider sandboxes (KYC/e-sign/payments); pact tests for internal APIs.

- 📦 **Local Dev Profiles**
  - Seed data, fake KYC/e-sign modes, toggleable OTP backends.

- 🧭 **API Docs & SDKs**
  - OpenAPI, Postman, language snippets, example webhooks.

---

## 🔗 Dependencies & Notes

- **Requires**: solid MVP foundation (Identity+YARP, Person, Catalog, Bids, WebApp, ClientApp).  
- **Security first**: no IMEI; device attestation; signed webhooks; rate-limits.  
- **KYC remains mandatory & first** across all future flows.

