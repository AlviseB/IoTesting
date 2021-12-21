# IoTesting

Basso Andrea,
Battistich Alvise

Client in C# - Server in Node.js con Framework Restify

---

## Client
- Scritto in C#
- Simula le richieste di un drone ed invia dati simulati al server

---

## Server
- Scritto in node.js e usa i Framework:
  - Restify
  - Mongoose
- Raccoglie i dati dei sensori dei droni e li salva con un marchio temporale su una collection dedicata
  ### Endpoints
  `GET /drones` Restituisce tutti i droni con i dati aggiornati
  
  `GET /drones/:ID` Restituisce il drone *ID*
  
  `POST /drones` Crea un drone con i dati inviati
  
  `GET /drones/:ID/action` Restituisce la prossima azione del drone *ID*
  
  `POST /drones/:ID/action` Crea un'azione che il drone *ID* eseguirà
