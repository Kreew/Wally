# Wally

Progetto sviluppato in Unity che prevede l'utilizzo della telecamenra 360° RICOH THETA Z1 (Firmware version 1.60.1) e della pedana Kat Walk Mini S. Il progetto prevede la realizzazione di una demo in cui è possibile telecomandare un robot(tramite l'input della pedana) equipaggiato con la telecamera che trasmette le immagini che è possibile visualizzare tramite un visore VR.

## Trasmissione della telecamera a Unity

La prima parte del progetto consiste nel portare in ambiente Unity la view della telecamera in real-time.

### Configurazione della telecamera

Per poter utilizzare le funzionalità wireless della telecamera è strettamente necessario l'utilizzo di un plug-in apposito. Per questo progetto è stato scelto : [RICOH THETA Z1 RTSP plug-in](https://github.com/ricohapi/theta-plugins/tree/main/plugins/com.sciencearts.rtspstreaming)

Per funzionare correttamente è necessario che la telecamera e il dispositivo client siano connessi ad un router che faccia da ponte.

Per configurare la connessione tra telecamera e router bisogna accedere alla telecamera via applicazione mobile:
- attivare la modalità AP - Access Point sulla telecamera
- connettersi con lo smartphone alla rete wi-fi della telecamera e poi connettersi tramite Modalità punto di accesso, le credenziali di accesso sono il codice sotto l'ingresso USB-C della telecamera.
- nella voce: Impostazioni della fotocamera -> Modalità client LAN wireless -> Impostazioni punto di accesso;  inserire le credenziali del router.
- cambiare la modalità di connessione della telecamera in CL Connection Link 
- collegare il telefono al router e collegarsi tramite l'app usando la voce Modalità client LAN wireless
- nella voce: Impostazioni della fotocamera. -> Versione fotocamera; è possibile reperire l'indirizzo Ip della telecamera necessario per stabilire la connessione.

Accertarsi di eventuali problemi di firewall.


### WALLY-OBS

Questo progetto utilizza OBS come client RTSP per poi trasmettere l'output del video tramite una camera virtuale che è possibile reperire facilmente tra i devices disponibili. Potrebbe essere necessario modificare il riferimento al device in caso di non funzionamento.

Per abilitare la connessione RTSP su OBS aprire una nuova source di tipo "Media Source" e inserire nel campo input: rtsp://[your RICOH THETA IP Address]:8554/live?resolution=[your resolution].



### WALLY-NUGET

Questo progetto utilizza il package [RTSPClientSharp](https://www.nuget.org/packages/RtspClientSharp/) per aprire un client e stabilire una connessione diretta con la telecamera. Attualmente l'applicazione apre correttamente il processo client e riceve i frame, tuttavia non è stato ancora possibile decodificare i frame tramite le librerie e il codice fornito da esempio. Per una corretta connessione è necessario avviare prima il plug-in della telecamera e successivamente il client altrimenti potrebbero esserci tempi elevati per la connessione.

Nel caso si riesca a decodificare i frame questa soluzione potrebbe risultare molto più performante in termini di latenza.


## Trasmissione dell'input dalla pedana al robot

Seconda parte del progetto e consiste nell'integrazione della pedana all'ambiente e la comunicazione dell'input al robot.

### TODO...

