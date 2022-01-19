# IoTesting

Basso Andrea,
Battistich Alvise

---

## Client
Scritto in **C#**

Simula sensori di velocità, altitudine, orientamento nei tre assi (x,y,z), GPS (latitudine e longitudine), percentuale della batteria residua.

All'avvio dell'applicazione vengono lanciati due **thread**:
  - **invio dei dati dei sensori**; 
  - **lettura dei comandi da eseguire**;
 
 Il thread per l'invio dei dati dei sensori richiama il metodo *send* del protocollo specificato con cadenza pari a 1 secondo. Questa scelta è legata al tempo a disposizione per lo sviluppo, ma una soluzione migliore sarebbe stata **modificare la frequenza di chiamate al server in relazione allo stato del drone**.
 Il thread per la lettura dei comandi da eseguire richiama il metodo *received* del protocollo specificato. 
 
L'utilizzo di thread separati permette la scelta di due **protocolli indipendenti per invio dei dati e ricezione dei comandi**.

Il drone viene identificato per semplicità di sviluppo da una stringa univoca (ID), definita nel **file di configurazione** dell'applicazione (app.config). In questo modo è possibile modificare facilmente questo parametro in fase di assemblaggio del singolo drone. Soluzioni più efficienti prevederebbero dei sistemi che ne garantiscano l'univocità, come ad esempio l'utilizzo dell'indirizzo MAC del dispositivo.

Sono state inserite nel file di configurazione anche le seguenti variabili:
  - *location*, dove troviamo impostata la località della sede da dove viene noleggiato il drone, immaginiamo infatti la possibilità di avere più sedi;
  - *company*, il nome della compagnia (nel nostro caso abbiamo usato il nome del corso - iot2021)
  - *MQTT_version*, la versione con cui vengono scambiati i messaggi con il protocollo MQTT

  ### HTTP
  Descrizione dei metodi utilizzati dal protocollo HTTP:
  - *send*, i dati vengono inviati sull'endpoint `POST /drones` come un'**unica stringa in formato JSON**, comprensivi di timestamp e id del drone. Tale soluzione permette di **ottimizzare le chiamate** al server, nonché le scritture sul database.
  - *received*, richiede con cadenza pari a 1 secondo il comando da eseguire al server sull'endpoint `GET /drones/:ID/action`, deserializza il JSON utilizzando il metodo statico *deserializeCommand* della classe *JsonManager* e lo stampa in console per fini didattici - i comandi andrebbero in una soluzione ideale eseguiti dal drone, prima di richiedere il comando successivo.

  ### MQTT
  Descrizione dei metodi utilizzati dal protocollo MQTT:
  - *send*, pubblica sul topic `company/version/luogo/drone/status/sensor`, dove *status* è costante, una **stringa in formato json per ogni sensore** con il valore letto dal sensore. La scelta di inviare il dato in json e non il singolo valore del sensore è stata determinata dalla presenza di sensori con dati complessi (come il GPS che ha i dati di latitudine e longitudine), nonché per rendere possibili aggiornamenti futuri che necessitino dell'**invio di dati completi**, come ad esempio un resoconto dello stato del drone. Siamo consapevoli che questa scelta determini un aumento, seppur minimo poichè si parla di pochi caratteri, del peso dei singoli payload.
  - *received*, si iscrive al topic `company/version/luogo/drone/command`, dove *command* è costante, e resta in attesa di comandi. Come per il protocollo HTTP stampiamo il comando su console per fini didattici.
  
  ### CoAP
  Descrizione dei metodi utilizzati dal protocollo CoAP:
  - *send*, come per il protocollo HTTP, i dati vengono inviati come un'**unica stringa in formato JSON** sull'endpoint `POST /drones`, comprensivi di timestamp e id del drone.
  - *received*, come per il protocollo HTTP, richiede ogni secondo sull'endpoint `GET /drones/:ID/action` il comando da eseguire al server, deserializza il JSON utilizzando il metodo statico *deserializeCommand* della classe *JsonManager* e lo stampa in console. Una soluzione ottimale, non realizzata per mancanza di tempo e assenza di metodi adatti nella libreria, prevedrebbe la **sottoscrizione tramite Observe alla risorsa** sul medesimo endpoint utilizzato.
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

  ### Endpoints HTTP
  `GET /drones` Restituisce tutti i droni con i relativi dati aggiornati
  
  `GET /drones/:ID` Restituisce i dati più recenti dei sensori del drone identificato dal parametro *ID*
  
  `POST /drones` Inserisce un nuovo record contenente i dati dei sensori relativi al drone che li invia
  
  `GET /drones/:ID/action` Restituisce la prossima azione da eseguire per il drone identificato da *ID*
  
  `POST /drones/:ID/action` Inserisce in coda un'azione da eseguire per il drone identificato da *ID*
  
  ### Topic MQTT
  `*company*/*version*/*luogo*/*drone*/command`, il server pubblica su questo topic i comandi da far eseguire al drone. Questa operazione potrebbe essere fatta da un altro dispositivo.
  
  `*company*/*version*/*luogo*/*drone*/status/*sensor*`, il server si iscrive a questo topic per ogni drone, ricevendo il payload di ogni sensore e inserendolo nel DB.
  
  ### Endpoints CoAP
  `POST /drones` Inserisce un nuovo record contenente i dati dei sensori relativi al drone che li invia
  `GET /drones/:ID/action` Restituisce la prossima azione da eseguire per il drone identificato da *ID*
  
