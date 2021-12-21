# IoTesting

Basso Andrea,
Battistich Alvise

---

## Client
Scritto in **C#**

Simula sensori di velocità, altitudine, orientamento nei tre assi (x,y,z), GPS (latitudine e longitudine), percentuale della batteria residua.

All'avvio dell'applicazione vengono lanciati due **thread**:
  - **invio dei dati dei sensori**; invia i dati come un'unica stringa in formato JSON, comprensivi di timestamp e id del drone. Tale soluzione permette di ottimizzare le chiamate al server, nonché le scritture sul database.
  - **lettura dei comandi da eseguire**; richiede il comando da eseguire al server, deserializza il JSON utilizzando la libreria Newtonsoft JSON.NET e lo stampa in console per fini didattici - i comandi andrebbero in una soluzione ideale eseguiti dal drone, prima di richiedere il comando successivo. 
 
Ogni thread attende 1 secondo prima di inviare nuovamente i dati/leggere il comando. Questa scelta è legata al tempo a disposizione per lo sviluppo, ma una soluzione migliore sarebbe stata **modificare la frequenza di chiamate al server in relazione allo stato del drone** e alla velocità di esecuzione dei comandi.

Il drone viene identificato per semplicità di sviluppo da una stringa univoca (ID), definita nella funzione Main() dell'applicativo. Soluzioni più efficienti prevederebbero dei sistemi che ne garantiscano l'univocità, come ad esempio l'utilizzo dell'indirizzo MAC del dispositivo.

---

## Server
Scritto in **node.js**, sfruttando:
  - il framework **Restify**
  - la libreria **Mongoose**, per la gestione del database MongoDB
 
Raccoglie i dati dei sensori dei droni e li salva con un marchio temporale su una collection dedicata del db non relazionale MongoDB. 

La scelta del database MongoDB è dipesa dalle tempistiche di sviluppo e da problemi di connessione al database Microsoft SQL Server. Consideriamo comunque migliore una **soluzione che sfrutti un database di tipo relazionale**, con le seguenti tabelle:
  - **utenti**, contiene dati sugli utenti
  - **droni**, contiene i droni e informazioni relative al modello
  - **noleggi**, contiene dati sui noleggi (id dell'utente e del drone noleggiato, data di noleggio, durata, ecc. ecc.)
  - **storico dei dati raccolti**, contiene lo storico dei dati inviati dai droni (e id del drone)

  ### Endpoints
  `GET /drones` Restituisce tutti i droni con i dati aggiornati
  
  `GET /drones/:ID` Restituisce il drone *ID*
  
  `POST /drones` Crea un drone con i dati inviati
  
  `GET /drones/:ID/action` Restituisce la prossima azione del drone *ID*
  
  `POST /drones/:ID/action` Crea un'azione che il drone *ID* eseguirà
