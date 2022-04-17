using Structurizr;
using Structurizr.Api;

namespace fas_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 73409;
            const string apiKey = "30c340fe-71a7-4b48-8ae9-4eb2b2c335f9";
            const string apiSecret = "0768462f-1e1b-4c82-beed-a98a36def98c";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("FAS C4 Model - FrikiEvents", "Plataforma de eventos friki");
            Model model = workspace.Model;
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SoftwareSystem frikiEventsSystem = model.AddSoftwareSystem("Friki Events", "Plataforma orientada a encontrar y publicar eventos de indole friki");
            SoftwareSystem googleMaps = model.AddSoftwareSystem("Google Maps API", "Brinda información de geolocalización, mapas, etc");
            SoftwareSystem paySystem = model.AddSoftwareSystem("Mercado Pago API", "Pasarela de pagos compatible con múltiples métodos de pago");
            SoftwareSystem emailSystem = model.AddSoftwareSystem("Email Systen", "Sistema de mensajería");

            Person organizador = model.AddPerson("Organizador de eventos", "Usuario que publica eventos para los usuarios friki");
            Person usuarioFriki = model.AddPerson("Usuario friki", "Usuario que utiliza la plataforma para encontrar eventos friki");

            organizador.Uses(frikiEventsSystem, "Publica los eventos friki en");
            usuarioFriki.Uses(frikiEventsSystem, "Encuentra los eventos friki en");
            frikiEventsSystem.Uses(paySystem, "Realiza peticiones para la compra y venta de entradas a un evento friki");
            frikiEventsSystem.Uses(googleMaps, "Obtiene información de geolocalización de los usuarios y los eventos friki");
            frikiEventsSystem.Uses(emailSystem, "Envia correos usando");


            SystemContextView contextView = viewSet.CreateSystemContextView(frikiEventsSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            frikiEventsSystem.AddTags("SistemaMonitoreo");
            googleMaps.AddTags("GoogleMaps");
            paySystem.AddTags("AircraftSystem");
            emailSystem.AddTags("EmailSystem");
            organizador.AddTags("Ciudadano");
            usuarioFriki.AddTags("Periodista");
            
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Ciudadano") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Periodista") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaMonitoreo") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("GoogleMaps") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("AircraftSystem") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("EmailSystem") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            //Container mobileApplication = monitoringSystem.AddContainer("Mobile App", "Permite a los usuarios visualizar un dashboard con el resumen de toda la información del traslado de los lotes de vacunas.", "Flutter");
            Container webApplication = frikiEventsSystem.AddContainer("Web Application", "Permite a los organizadores publicar eventos friki y a los usuarios friki encontrarlos", "Angular");
            Container landingPage = frikiEventsSystem.AddContainer("Landing Page", "Página estática que proporciona información acerca de FrikiEvents", "HTML, CSS Y Javascript");
            Container apiGateway = frikiEventsSystem.AddContainer("API Gateway", "API Gateway", "Spring Boot port 8080");
            Container userBoundedContext = frikiEventsSystem.AddContainer("User Bounded context", "", "Java and Spring Boot");
            Container socialNetworkingBoundedContext = frikiEventsSystem.AddContainer("Social Network Bounded Context", "", "Java and Spring Boot");
            Container paymentBoundedContext = frikiEventsSystem.AddContainer("Payment Bounded Context", "", "Java and Spring Boot");
            Container eventBoundedContext = frikiEventsSystem.AddContainer("Event Bounded Context", "", "Java and Spring Boot");
            // Container vaccinesInventoryContext = monitoringSystem.AddContainer("Vaccines Inventory Context", "Bounded Context del Microservicio de Inventario de Vacunas", "Spring Boot port 8084");
            // Container monitoringContext = monitoringSystem.AddContainer("Monitoring Context", "Bounded Context del Microservicio de Monitoreo en tiempo real del status y ubicación del vuelo que transporta las vacunas", "Spring Boot port 8085");
            Container messageBus =
                frikiEventsSystem.AddContainer("Bus de Mensajes en Cluster de Alta Disponibilidad", "Transporte de eventos del dominio.", "RabbitMQ");
            // Container flightPlanningContextDatabase = frikiEventsSystem.AddContainer("Database", "Almacena la informacion de los postulantes, empleadores, ofertas de trabajo, entrevistas, etc", "Oracle");
            Container userContextDatabase = frikiEventsSystem.AddContainer("User Context DB", "", "Oracle");
            Container socialNetworkContextDatabase = frikiEventsSystem.AddContainer("Social Network Context DB", "", "Oracle");
            Container paymentContextDatabase = frikiEventsSystem.AddContainer("Payment Context DB", "", "Oracle");
            Container eventContextDatabase = frikiEventsSystem.AddContainer("Event Context DB", "", "Oracle");

            // Container monitoringContextDatabase = monitoringSystem.AddContainer("Monitoring Context DB", "", "Oracle");
            // Container monitoringContextReplicaDatabase = monitoringSystem.AddContainer("Monitoring Context DB Replica", "", "Oracle");
            // Container monitoringContextReactiveDatabase = monitoringSystem.AddContainer("Monitoring Context Reactive DB", "", "Firebase Cloud Firestore");
            
            // ciudadano.Uses(mobileApplication, "Consulta");
            organizador.Uses(webApplication, "Publica eventos friki en");
            organizador.Uses(landingPage, "Visita frikievents.com usando");
            landingPage.Uses(webApplication, "Redirige a");
            // periodista.Uses(mobileApplication, "Consulta");
            usuarioFriki.Uses(webApplication, "Encuentra eventos friki en");
            usuarioFriki.Uses(landingPage,  "Visita frikievents.com usando");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            // webApplication.Uses(userBoundedContext, "API Request", "JSON/HTTPS");
            // webApplication.Uses(socialNetworkingBoundedContext, "API Request", "JSON/HTTPS");
            // webApplication.Uses(paymentBoundedContext, "API Request", "JSON/HTTPS");

            apiGateway.Uses(userBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(socialNetworkingBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(paymentBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(eventBoundedContext, "API Request", "JSON/HTTPS");
            // apiGateway.Uses(monitoringContext, "API Request", "JSON/HTTPS");
            userBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            userBoundedContext.Uses(userContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            userBoundedContext.Uses(googleMaps, "Obtiene los datos de geolocalización del usuario friki usando", "JSON/HTTPS");

            socialNetworkingBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            socialNetworkingBoundedContext.Uses(socialNetworkContextDatabase, "Create, Update, Delete and Get from", "JDBC");

            paymentBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            paymentBoundedContext.Uses(paymentContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            paymentBoundedContext.Uses(paySystem, "Gestiona el pago de compra de tickets usando", "JSON/HTTPS");

            eventBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            eventBoundedContext.Uses(eventContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            eventBoundedContext.Uses(emailSystem, "Envia correos acerca de los eventos friki usando", "JSON/HTTPS");
            eventBoundedContext.Uses(googleMaps, "Obtiene los datos de geolocalización de los eventos friki usando", "JSON/HTTPS");


            // airportContext.Uses(messageBus, "Publica y consume eventos del dominio");
            // socialNetworkingBoundedContext.Uses(flightPlanningContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            // aircraftInventoryContext.Uses(messageBus, "Publica y consume eventos del dominio");
            // paymentBoundedContext.Uses(flightPlanningContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            // vaccinesInventoryContext.Uses(messageBus, "Publica y consume eventos del dominio");
            // vaccinesInventoryContext.Uses(vaccinesInventoryContextDatabase, "", "JDBC");
            // monitoringContext.Uses(messageBus, "Publica y consume eventos del dominio");
            // userBoundedContext.Uses(paySystem , "Almacena los documentos para la postulacion (CV, Carta de presentacion, video de presentacion)", "JSON/HTTPS");
            // socialNetworkingBoundedContext.Uses(paySystem , "Almacena los  documentos del postulante como foto de perfil", "JSON/HTTPS");
            // paymentBoundedContext.Uses(paySystem , "Almacena los  documentos del postulante como foto de perfil de la empresa", "JSON/HTTPS");
            // userBoundedContext.Uses(googleMaps , "Genera el link para la videoconferencia", "JSON/HTTPS");
            // monitoringContextDatabase.Uses(monitoringContextReplicaDatabase, "Replica");
            // monitoringContext.Uses(googleMaps, "API Request", "JSON/HTTPS");
            // monitoringContext.Uses(aircraftSystem, "API Request", "JSON/HTTPS");

            // Tags
            // mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");
            userBoundedContext.AddTags("FlightPlanningContext");
            socialNetworkingBoundedContext.AddTags("FlightPlanningContext");
            paymentBoundedContext.AddTags("FlightPlanningContext");
            eventBoundedContext.AddTags("FlightPlanningContext");

            userContextDatabase.AddTags("FlightPlanningContextDatabase");
            socialNetworkContextDatabase.AddTags("FlightPlanningContextDatabase");
            paymentContextDatabase.AddTags("FlightPlanningContextDatabase");
            eventContextDatabase.AddTags("FlightPlanningContextDatabase");

            // socialNetworkingBoundedContext.AddTags("AirportContext");
            // airportContextDatabase.AddTags("AirportContextDatabase");
            // paymentBoundedContext.AddTags("AircraftInventoryContext");
            // aircraftInventoryContextDatabase.AddTags("AircraftInventoryContextDatabase");
            // vaccinesInventoryContext.AddTags("VaccinesInventoryContext");
            // vaccinesInventoryContextDatabase.AddTags("VaccinesInventoryContextDatabase");
            // monitoringContext.AddTags("MonitoringContext");
            // monitoringContextDatabase.AddTags("MonitoringContextDatabase");
            // monitoringContextReplicaDatabase.AddTags("MonitoringContextReplicaDatabase");
            // monitoringContextReactiveDatabase.AddTags("MonitoringContextReactiveDatabase");
            messageBus.AddTags("MessageBus");
            
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            // styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("FlightPlanningContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightPlanningContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            // styles.Add(new ElementStyle("AirportContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("AirportContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("AircraftInventoryContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("AircraftInventoryContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            // styles.Add(new ElementStyle("VaccinesInventoryContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("VaccinesInventoryContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringContextReplicaDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringContextReactiveDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("MessageBus") { Width = 850, Background = "#fd8208", Color = "#ffffff", Shape = Shape.Pipe, Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(frikiEventsSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes
            //Payment bounded context

            // Component domainLayer = airportContext.AddComponent("Domain Layer", "", "Spring Boot");
            // Component monitoringController = socialNetworkingBoundedContext.AddComponent("Language Controller", "Controller de los idiomas que habla el postulante", "Spring Boot REST Controller");
            // Component monitoringApplicationService = socialNetworkingBoundedContext.AddComponent("Lenguage Service", "Provee métodos para los idiomas que habla el postulante", "Spring Component");
            // Component flightRepository = socialNetworkingBoundedContext.AddComponent("Lenguage Repository", "Provee métodos para la persistencia de datos para los idiomas que habla el postulante", "Spring Component");
            
            Component offersController = paymentBoundedContext.AddComponent("Offers Controller", "Controller de las ofertas para un evento friki", "Spring Boot REST Controller");
            Component offersApplicationService = paymentBoundedContext.AddComponent("Offers Service", "Provee métodos para manejar las ofertas para un evento friki", "Spring Component");
            Component offersRepository = paymentBoundedContext.AddComponent("Offers Repository", "Provee métodos para la persistencia de las ofertas para un evento friki", "Spring Component");

            Component ticketsController = paymentBoundedContext.AddComponent("Tickets Controller", "Controller para el manejo de los tickets para los eventos", "Spring Boot REST Controller");
            Component ticketsApplicationService = paymentBoundedContext.AddComponent("Tickets Service", "Provee métodos para el manejo de los tickets para los eventos", "Spring Component");
            Component ticketsRepository = paymentBoundedContext.AddComponent("Tickets Repository", "Provee métodos para la persistencia de datos del manejo de los tickets para los eventos", "Spring Component");

            Component paymentMethodsController = paymentBoundedContext.AddComponent("Payment methods Controller", "Controller para el manejo de los métodos de pago para los eventos", "Spring Boot REST Controller");
            Component paymentMethodsApplicationService = paymentBoundedContext.AddComponent("Payment methods Service", "Provee métodos para manejar la información de los métodos de pago para los eventos", "Spring Component");
            Component paymentMethodsRepository = paymentBoundedContext.AddComponent("Payment methods Repository", "Provee métodos para la persistencia de datos de de los métodos de pago para los eventos", "Spring Component");

            Component paymentsController = paymentBoundedContext.AddComponent("Payments Controller", "Controller de los pagos para la asistencia a un evento", "Spring Boot REST Controller");
            Component paymentsApplicationService = paymentBoundedContext.AddComponent("Payments Service", "Provee métodos para manejar la información de los pagos para la asistencia a un evento", "Spring Component");
            Component paymentsRepository = paymentBoundedContext.AddComponent("Payments Repository", "Provee métodos para la persistencia de datos de los pagos para la asistencia a un evento", "Spring Component");

            // monitoringController.Uses(monitoringApplicationService, "Llama a los metodos del service");
            // monitoringApplicationService.Uses(flightRepository, "Llama a los metodos del repository");
            // flightRepository.Uses(flightPlanningContextDatabase, "Escribe y lee en");
            apiGateway.Uses(paymentsController, "", "JSON/HTTPS");
            apiGateway.Uses(offersController, "", "JSON/HTTPS");
            apiGateway.Uses(ticketsController, "", "JSON/HTTPS");
            apiGateway.Uses(paymentMethodsController, "", "JSON/HTTPS");

            offersController.Uses(offersApplicationService, "Llama a los metodos del service");
            offersApplicationService.Uses(offersRepository, "Llama a los metodos del repository");
            offersRepository.Uses(paymentContextDatabase, "Escribe y lee en");

            ticketsController.Uses(ticketsApplicationService, "Llama a los metodos del service");
            ticketsApplicationService.Uses(ticketsRepository, "Llama a los metodos del repository");
            // postulantApplicationService.Uses(paySystem, "Sube los archivos del postulante a");
            ticketsRepository.Uses(paymentContextDatabase, "Escribe y lee en");

            paymentMethodsController.Uses(paymentMethodsApplicationService, "Llama a los metodos del service");
            paymentMethodsApplicationService.Uses(paymentMethodsRepository, "Llama a los metodos del repository");
            paymentMethodsRepository.Uses(paymentContextDatabase, "Escribe y lee en");

            paymentsController.Uses(paymentsApplicationService, "Llama a los metodos del service");
            paymentsApplicationService.Uses(paymentsRepository, "Llama a los metodos del repository");
            paymentsApplicationService.Uses(paySystem, "Gestiona el pago de entrada para los eventos frikis");
            paymentsRepository.Uses(paymentContextDatabase, "Escribe y lee en");


            offersController.AddTags("MonitoringController");
            offersApplicationService.AddTags("MonitoringApplicationService");
            offersRepository.AddTags("FlightRepository");

            ticketsController.AddTags("MonitoringController");
            ticketsApplicationService.AddTags("MonitoringApplicationService");
            ticketsRepository.AddTags("FlightRepository");

            paymentMethodsController.AddTags("MonitoringController");
            paymentMethodsApplicationService.AddTags("MonitoringApplicationService");
            paymentMethodsRepository.AddTags("FlightRepository");

            paymentsController.AddTags("MonitoringController");
            paymentsApplicationService.AddTags("MonitoringApplicationService");
            paymentsRepository.AddTags("FlightRepository");
            // Component vaccineLoteRepository = airportContext.AddComponent("VaccineLote Repository", "Información de lote de vacunas", "Spring Component");
            // Component locationRepository = airportContext.AddComponent("Location Repository", "Ubicación del vuelo", "Spring Component");
            // Component aircraftSystemFacade = airportContext.AddComponent("Aircraft System Facade", "", "Spring Component");

            // apiGateway.Uses(monitoringController, "", "JSON/HTTPS");
            // monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");
            // monitoringController.Uses(aircraftSystemFacade, "Usa");
            // monitoringApplicationService.Uses(domainLayer, "Usa", "");
            // monitoringApplicationService.Uses(flightRepository, "", "JDBC");
            // monitoringApplicationService.Uses(vaccineLoteRepository, "", "JDBC");
            // monitoringApplicationService.Uses(locationRepository, "", "JDBC");
            // flightRepository.Uses(monitoringContextDatabase, "", "JDBC");
            // vaccineLoteRepository.Uses(monitoringContextDatabase, "", "JDBC");
            // locationRepository.Uses(monitoringContextDatabase, "", "JDBC");
            // locationRepository.Uses(monitoringContextReactiveDatabase, "", "");
            // locationRepository.Uses(googleMaps, "", "JSON/HTTPS");
            // aircraftSystemFacade.Uses(aircraftSystem, "JSON/HTTPS");
            
            // Tags
            // domainLayer.AddTags("DomainLayer");
            // monitoringController.AddTags("MonitoringController");
            // monitoringApplicationService.AddTags("MonitoringApplicationService");
            // flightRepository.AddTags("FlightRepository");



            // vaccineLoteRepository.AddTags("VaccineLoteRepository");
            // locationRepository.AddTags("LocationRepository");
            // aircraftSystemFacade.AddTags("AircraftSystemFacade");
            
            // styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(paymentBoundedContext, "Payment Components", "Payment Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(paySystem);
            componentView.Add(apiGateway);
            componentView.Add(webApplication);
            componentView.Add(paymentContextDatabase);
            componentView.AddAllComponents();
            // componentView.Add(googleMaps);
            // // componentView.Add(mobileApplication);
            // componentView.Add(webApplication);
            // componentView.Add(apiGateway);
            // componentView.Add(monitoringContextDatabase);
            // componentView.Add(flightPlanningContextDatabase);
            // componentView.Add(monitoringContextReactiveDatabase);



            //Social Networking bounded context
            
            Component eventCommentsController = socialNetworkingBoundedContext.AddComponent("Comments Controller", "Controller de los comentarios dados a un evento friki", "Spring Boot REST Controller");
            Component eventCommentsService = socialNetworkingBoundedContext.AddComponent("Comments Service", "Provee métodos para manejar los comentarios dados a un evento friki", "Spring Component");
            Component eventCommentsRepository = socialNetworkingBoundedContext.AddComponent("Comments Repository", "Provee métodos para la persistencia de datos de los comentarios dados a un evento friki", "Spring Component");

            Component qualificationsController = socialNetworkingBoundedContext.AddComponent("Qualifications Controller", "Controller para el manejo de la calificacion dada a los eventos friki", "Spring Boot REST Controller");
            Component qualificationsApplicationService = socialNetworkingBoundedContext.AddComponent("Qualifications Service", "Provee métodos para el manejo de la calificacion dada a los eventos friki", "Spring Component");
            Component qualificationsRepository = socialNetworkingBoundedContext.AddComponent("Qualifications Repository", "Provee métodos para la persistencia de datos de la calificacion dada a los eventos friki", "Spring Component");

            Component socialNetworkController = socialNetworkingBoundedContext.AddComponent("Social Network Controller", "Controller para el manejo de las redes sociales de los eventos", "Spring Boot REST Controller");
            Component socialNetworkApplicationService = socialNetworkingBoundedContext.AddComponent("Social Network Service", "Provee métodos para manejar la información de las redes sociales de los eventos", "Spring Component");
            Component socialNetworkRepository = socialNetworkingBoundedContext.AddComponent("Social Network Repository", "Provee métodos para la persistencia de datos de de las redes sociales de los eventos", "Spring Component");

            Component organizerFollowsController = socialNetworkingBoundedContext.AddComponent("Organizer Follows Controller", "Controller del seguimiento a los eventos de un organizador", "Spring Boot REST Controller");
            Component organizerFollowsApplicationService = socialNetworkingBoundedContext.AddComponent("Organizer Follows Service", "Provee métodos para manejar la información de los eventos de un organizador", "Spring Component");
            Component organizerFollowsRepository = socialNetworkingBoundedContext.AddComponent("Organizer Follows Repository", "Provee métodos para la persistencia de datos de los eventos de un organizador", "Spring Component");
     
            Component eventFollowsController = socialNetworkingBoundedContext.AddComponent("Event Follows Controller", "Controller del seguimiento a un evento en especifico", "Spring Boot REST Controller");
            Component eventFollowsApplicationService = socialNetworkingBoundedContext.AddComponent("Event Follows Service", "Provee métodos para manejar la información del seguimiento a un evento en especifico", "Spring Component");
            Component eventFollowsRepository = socialNetworkingBoundedContext.AddComponent("Event Follows Repository", "Provee métodos para la persistencia de datos del seguimiento a un evento en especifico", "Spring Component");

            apiGateway.Uses(eventCommentsController, "", "JSON/HTTPS");
            apiGateway.Uses(qualificationsController, "", "JSON/HTTPS");
            apiGateway.Uses(socialNetworkController, "", "JSON/HTTPS");
            apiGateway.Uses(organizerFollowsController, "", "JSON/HTTPS");
            apiGateway.Uses(eventFollowsController, "", "JSON/HTTPS");

            eventCommentsController.Uses(eventCommentsService, "Llama a los metodos del service");
            eventCommentsService.Uses(eventCommentsRepository, "Llama a los metodos del repository");
            eventCommentsRepository.Uses(socialNetworkContextDatabase, "Escribe y lee en");

            qualificationsController.Uses(qualificationsApplicationService, "Llama a los metodos del service");
            qualificationsApplicationService.Uses(qualificationsRepository, "Llama a los metodos del repository");
            qualificationsRepository.Uses(socialNetworkContextDatabase, "Escribe y lee en");

            socialNetworkController.Uses(socialNetworkApplicationService, "Llama a los metodos del service");
            socialNetworkApplicationService.Uses(socialNetworkRepository, "Llama a los metodos del repository");
            socialNetworkRepository.Uses(socialNetworkContextDatabase, "Escribe y lee en");

            organizerFollowsController.Uses(organizerFollowsApplicationService, "Llama a los metodos del service");
            organizerFollowsApplicationService.Uses(organizerFollowsRepository, "Llama a los metodos del repository");
            organizerFollowsRepository.Uses(socialNetworkContextDatabase, "Escribe y lee en");

            eventFollowsController.Uses(eventFollowsApplicationService, "Llama a los metodos del service");
            eventFollowsApplicationService.Uses(eventFollowsRepository, "Llama a los metodos del repository");
            eventFollowsRepository.Uses(socialNetworkContextDatabase, "Escribe y lee en");


            eventCommentsController.AddTags("MonitoringController");
            eventCommentsService.AddTags("MonitoringApplicationService");
            eventCommentsRepository.AddTags("FlightRepository");

            qualificationsController.AddTags("MonitoringController");
            qualificationsApplicationService.AddTags("MonitoringApplicationService");
            qualificationsRepository.AddTags("FlightRepository");

            socialNetworkController.AddTags("MonitoringController");
            socialNetworkApplicationService.AddTags("MonitoringApplicationService");
            socialNetworkRepository.AddTags("FlightRepository");

            organizerFollowsController.AddTags("MonitoringController");
            organizerFollowsApplicationService.AddTags("MonitoringApplicationService");
            organizerFollowsRepository.AddTags("FlightRepository");

            eventFollowsController.AddTags("MonitoringController");
            eventFollowsApplicationService.AddTags("MonitoringApplicationService");
            eventFollowsRepository.AddTags("FlightRepository");
            // Component vaccineLoteRepository = airportContext.AddComponent("VaccineLote Repository", "Información de lote de vacunas", "Spring Component");
            // Component locationRepository = airportContext.AddComponent("Location Repository", "Ubicación del vuelo", "Spring Component");
            // Component aircraftSystemFacade = airportContext.AddComponent("Aircraft System Facade", "", "Spring Component");

            // apiGateway.Uses(monitoringController, "", "JSON/HTTPS");
            // monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");
            // monitoringController.Uses(aircraftSystemFacade, "Usa");
            // monitoringApplicationService.Uses(domainLayer, "Usa", "");
            // monitoringApplicationService.Uses(flightRepository, "", "JDBC");
            // monitoringApplicationService.Uses(vaccineLoteRepository, "", "JDBC");
            // monitoringApplicationService.Uses(locationRepository, "", "JDBC");
            // flightRepository.Uses(monitoringContextDatabase, "", "JDBC");
            // vaccineLoteRepository.Uses(monitoringContextDatabase, "", "JDBC");
            // locationRepository.Uses(monitoringContextDatabase, "", "JDBC");
            // locationRepository.Uses(monitoringContextReactiveDatabase, "", "");
            // locationRepository.Uses(googleMaps, "", "JSON/HTTPS");
            // aircraftSystemFacade.Uses(aircraftSystem, "JSON/HTTPS");
            
            // Tags
            // domainLayer.AddTags("DomainLayer");
            // monitoringController.AddTags("MonitoringController");
            // monitoringApplicationService.AddTags("MonitoringApplicationService");
            // flightRepository.AddTags("FlightRepository");



            // vaccineLoteRepository.AddTags("VaccineLoteRepository");
            // locationRepository.AddTags("LocationRepository");
            // aircraftSystemFacade.AddTags("AircraftSystemFacade");
            
            // styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView socialComponentView = viewSet.CreateComponentView(socialNetworkingBoundedContext, "Social Network Components", "Social Network Component Diagram");
            socialComponentView.PaperSize = PaperSize.A4_Landscape;
            // componentView.Add(paySystem);
            socialComponentView.Add(apiGateway);
            socialComponentView.Add(webApplication);
            socialComponentView.Add(socialNetworkContextDatabase);
            socialComponentView.AddAllComponents();


            // Component employeerController = paymentBoundedContext .AddComponent("Employeer Controller", "Controller de la informacion del empleador", "Spring Boot REST Controller");
            // Component employeerApplicationService = paymentBoundedContext .AddComponent("Employeer Service", "Provee métodos para manejar la informacion del empleador", "Spring Component");
            // Component flightemployeerRepository = paymentBoundedContext .AddComponent("Employeer Repository", "Provee métodos para la persistencia de datos de la informacion del empleador", "Spring Component");
            
            // Component companyController = paymentBoundedContext .AddComponent("Company Controller", "Controller de la informacion de la empresa", "Spring Boot REST Controller");
            // Component companyApplicationService = paymentBoundedContext .AddComponent("Company Service", "Provee métodos para manejar la informacion de la empresa", "Spring Component");
            // Component companyRepository = paymentBoundedContext .AddComponent("Company Repository", "Provee métodos para la persistencia de datos de la informacion de la empresa", "Spring Component");

            // Component sectorController = paymentBoundedContext .AddComponent("Sector Controller", "Controller de la informacion del sector", "Spring Boot REST Controller");
            // Component sectorApplicationService = paymentBoundedContext .AddComponent("Sector Service", "Provee métodos para manejar la informacion del sector", "Spring Component");
            // Component sectorRepository = paymentBoundedContext .AddComponent("Sector Repository", "Provee métodos para la persistencia de datos de la información del sector", "Spring Component");

            // employeerController.Uses(employeerApplicationService, "Llama a los metodos del service");
            // employeerApplicationService.Uses(flightemployeerRepository, "Llama a los metodos del repository");
            // employeerApplicationService.Uses(paySystem, "Sube los archivos del empleador y la compañia a");
            // flightemployeerRepository.Uses(flightPlanningContextDatabase, "Escribe y lee en");

            // companyController.Uses(companyApplicationService, "Llama a los metodos del service");
            // companyApplicationService.Uses(companyRepository, "Llama a los metodos del repository");
            // companyRepository.Uses(flightPlanningContextDatabase, "Escribe y lee en");

            // sectorController.Uses(sectorApplicationService, "Llama a los metodos del service");
            // sectorApplicationService.Uses(sectorRepository, "Llama a los metodos del repository");
            // sectorRepository.Uses(flightPlanningContextDatabase, "Escribe y lee en");
            
            // Tags
            // employeerController.AddTags("MonitoringController");
            // employeerApplicationService.AddTags("MonitoringApplicationService");
            // flightemployeerRepository.AddTags("FlightRepository");

            // companyController.AddTags("MonitoringController");
            // companyApplicationService.AddTags("MonitoringApplicationService");
            // companyRepository.AddTags("FlightRepository");

            // sectorController.AddTags("MonitoringController");
            // sectorApplicationService.AddTags("MonitoringApplicationService");
            // sectorRepository.AddTags("FlightRepository");
            
            // styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            // styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            // ComponentView employeerComponentView = viewSet.CreateComponentView(paymentBoundedContext , "Employeer Components", "Employeer Component Diagram");
            // employeerComponentView.PaperSize = PaperSize.A4_Landscape;
            // employeerComponentView.Add(flightPlanningContextDatabase);
            // employeerComponentView.Add(paySystem);
            // employeerComponentView.AddAllComponents();
            
            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            // ComponentView jobOfferComponentView = viewSet.CreateComponentView(userBoundedContext , "Job Offer Components", "Job Offer Component Diagram");
            // jobOfferComponentView.PaperSize = PaperSize.A4_Landscape;
            // // jobOfferComponentView.Add(flightPlanningContextDatabase);
            // jobOfferComponentView.Add(paySystem);
            // jobOfferComponentView.Add(googleMaps);
            // jobOfferComponentView.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}