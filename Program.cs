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
            SoftwareSystem emailSystem = model.AddSoftwareSystem("Email System", "Sistema de mensajería");

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
            Container webApplication = frikiEventsSystem.AddContainer("Web Application", "Permite a los organizadores publicar eventos friki y a los usuarios friki encontrarlos", "Angular");
            Container landingPage = frikiEventsSystem.AddContainer("Landing Page", "Página estática que proporciona información acerca de FrikiEvents", "HTML, CSS Y Javascript");
            Container apiGateway = frikiEventsSystem.AddContainer("API Gateway", "API Gateway", "Spring Boot port 8080");
            Container userBoundedContext = frikiEventsSystem.AddContainer("User Bounded context", "", "Java and Spring Boot");
            Container socialNetworkingBoundedContext = frikiEventsSystem.AddContainer("Social Network Bounded Context", "", "Java and Spring Boot");
            Container authenticationBoundedContext = frikiEventsSystem.AddContainer("Authentication Bounded Context", "", "Java and Spring Boot");
            Container paymentBoundedContext = frikiEventsSystem.AddContainer("Payment Bounded Context", "", "Java and Spring Boot");
            Container eventBoundedContext = frikiEventsSystem.AddContainer("Event Bounded Context", "", "Java and Spring Boot");
            Container messageBus =
                frikiEventsSystem.AddContainer("Bus de Mensajes en Cluster de Alta Disponibilidad", "Transporte de eventos del dominio.", "RabbitMQ");
            Container userContextDatabase = frikiEventsSystem.AddContainer("User Context DB", "", "Oracle");
            Container socialNetworkContextDatabase = frikiEventsSystem.AddContainer("Social Network Context DB", "", "Oracle");
            Container authenticationContextDatabase = frikiEventsSystem.AddContainer("Authentication Context DB", "", "Oracle");
            Container paymentContextDatabase = frikiEventsSystem.AddContainer("Payment Context DB", "", "Oracle");
            Container eventContextDatabase = frikiEventsSystem.AddContainer("Event Context DB", "", "Oracle");

            organizador.Uses(webApplication, "Publica eventos friki en");
            organizador.Uses(landingPage, "Visita frikievents.com usando");
            landingPage.Uses(webApplication, "Redirige a");
            usuarioFriki.Uses(webApplication, "Encuentra eventos friki en");
            usuarioFriki.Uses(landingPage,  "Visita frikievents.com usando");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");

            apiGateway.Uses(userBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(socialNetworkingBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(authenticationBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(paymentBoundedContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(eventBoundedContext, "API Request", "JSON/HTTPS");

            userBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            userBoundedContext.Uses(userContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            userBoundedContext.Uses(googleMaps, "Obtiene los datos de geolocalización del usuario friki usando", "JSON/HTTPS");

            socialNetworkingBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            authenticationBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            socialNetworkingBoundedContext.Uses(socialNetworkContextDatabase, "Create, Update, Delete and Get from", "JDBC");

            authenticationBoundedContext.Uses(authenticationContextDatabase, "Create, Update, Delete and Get from", "JDBC");


            paymentBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            paymentBoundedContext.Uses(paymentContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            paymentBoundedContext.Uses(paySystem, "Gestiona el pago de compra de tickets usando", "JSON/HTTPS");

            eventBoundedContext.Uses(messageBus, "Publica y consume eventos del dominio");
            eventBoundedContext.Uses(eventContextDatabase, "Create, Update, Delete and Get from", "JDBC");
            eventBoundedContext.Uses(emailSystem, "Envia correos acerca de los eventos friki usando", "JSON/HTTPS");
            eventBoundedContext.Uses(googleMaps, "Obtiene los datos de geolocalización de los eventos friki usando", "JSON/HTTPS");

            // Tags
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");
            userBoundedContext.AddTags("FlightPlanningContext");
            socialNetworkingBoundedContext.AddTags("FlightPlanningContext");
            authenticationBoundedContext.AddTags("FlightPlanningContext");
            paymentBoundedContext.AddTags("FlightPlanningContext");
            eventBoundedContext.AddTags("FlightPlanningContext");

            userContextDatabase.AddTags("FlightPlanningContextDatabase");
            socialNetworkContextDatabase.AddTags("FlightPlanningContextDatabase");
            authenticationContextDatabase.AddTags("FlightPlanningContextDatabase");
            paymentContextDatabase.AddTags("FlightPlanningContextDatabase");
            eventContextDatabase.AddTags("FlightPlanningContextDatabase");

            messageBus.AddTags("MessageBus");
            
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("FlightPlanningContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightPlanningContextDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("AircraftInventoryContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MessageBus") { Width = 850, Background = "#fd8208", Color = "#ffffff", Shape = Shape.Pipe, Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(frikiEventsSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes
            //Payment bounded context

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

            apiGateway.Uses(paymentsController, "", "JSON/HTTPS");
            apiGateway.Uses(offersController, "", "JSON/HTTPS");
            apiGateway.Uses(ticketsController, "", "JSON/HTTPS");
            apiGateway.Uses(paymentMethodsController, "", "JSON/HTTPS");

            offersController.Uses(offersApplicationService, "Llama a los metodos del service");
            offersApplicationService.Uses(offersRepository, "Llama a los metodos del repository");
            offersRepository.Uses(paymentContextDatabase, "Escribe y lee en");

            ticketsController.Uses(ticketsApplicationService, "Llama a los metodos del service");
            ticketsApplicationService.Uses(ticketsRepository, "Llama a los metodos del repository");
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

            ComponentView componentView = viewSet.CreateComponentView(paymentBoundedContext, "Payment Components", "Payment Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(paySystem);
            componentView.Add(apiGateway);
            componentView.Add(webApplication);
            componentView.Add(paymentContextDatabase);
            componentView.AddAllComponents();

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

            ComponentView socialComponentView = viewSet.CreateComponentView(socialNetworkingBoundedContext, "Social Network Components", "Social Network Component Diagram");
            socialComponentView.PaperSize = PaperSize.A4_Landscape;
            
            socialComponentView.Add(apiGateway);
            socialComponentView.Add(webApplication);
            socialComponentView.Add(socialNetworkContextDatabase);
            socialComponentView.AddAllComponents();



            // Authentication

            Component authenticationController =  authenticationBoundedContext.AddComponent("Authentication Controller", "Controller para la autenticacion de los usuarios", "Spring Boot REST Controller");
            Component authenticationService =     authenticationBoundedContext.AddComponent("Authentication Service", "Provee métodos para la autenticacion de los usuarios", "Spring Component");
            Component authenticationRepository =  authenticationBoundedContext.AddComponent("Authentication Repository", "Provee métodos para la persistencia de las identidades de los usuarios", "Spring Component");

            apiGateway.Uses(authenticationController, "", "JSON/HTTPS");


            authenticationController.Uses(authenticationService, "Llama a los metodos del service");
            authenticationService.Uses(authenticationRepository, "Llama a los metodos del repository");
            authenticationRepository.Uses(authenticationContextDatabase, "Escribe y lee en");


            authenticationController.AddTags("MonitoringController");
            authenticationService.AddTags("MonitoringApplicationService");
            authenticationRepository.AddTags("FlightRepository");

            ComponentView authenticationComponentView = viewSet.CreateComponentView(authenticationBoundedContext, "Authentication Components", "Authentication Component Diagram");
            authenticationComponentView.PaperSize = PaperSize.A4_Landscape;
            authenticationComponentView.PaperSize = PaperSize.A4_Landscape;
            
            authenticationComponentView.Add(apiGateway);
            authenticationComponentView.Add(webApplication);
            authenticationComponentView.Add(authenticationContextDatabase);
            authenticationComponentView.AddAllComponents();

            
            //User bounded context

            Component customersController =  userBoundedContext.AddComponent("Customer Controller", "Controller de los usuarios frikis", "Spring Boot REST Controller");
            Component customersService =     userBoundedContext.AddComponent("Customer Service", "Provee métodos para manejar a los usuarios frikis", "Spring Component");
            Component customersRepository =  userBoundedContext.AddComponent("Customer Repository", "Provee métodos para la persistencia de datos de los usuarios frikis", "Spring Component");

            Component organizersController =  userBoundedContext.AddComponent("Organizer Controller", "Controller de los organizadores de eventos", "Spring Boot REST Controller");
            Component organizersService =     userBoundedContext.AddComponent("Organizer Service", "Provee métodos para manejar a los organizadores de eventos", "Spring Component");
            Component organizersRepository =  userBoundedContext.AddComponent("Organizer Repository", "Provee métodos para la persistencia de datos de los organizadores de eventos", "Spring Component");


            apiGateway.Uses(customersController, "", "JSON/HTTPS");
            apiGateway.Uses(organizersController, "", "JSON/HTTPS");

            customersController.Uses(customersService, "Llama a los metodos del service");
            customersService.Uses(customersRepository, "Llama a los metodos del repository");
            /*ojo*/
            customersService.Uses(emailSystem, "Administra el envio de correos al usuario friki");
            customersRepository.Uses(userContextDatabase, "Escribe y lee en");

            organizersController.Uses(organizersService, "Llama a los metodos del service");
            organizersService.Uses(organizersRepository, "Llama a los metodos del repository");
            /*ojo*/
            organizersService.Uses(emailSystem, "Administra el envio de correos al usuario organizador");
            organizersRepository.Uses(userContextDatabase, "Escribe y lee en");
            
            customersController.AddTags("MonitoringController");
            customersService.AddTags("MonitoringApplicationService");
            customersRepository.AddTags("FlightRepository");

            organizersController.AddTags("MonitoringController");
            organizersService.AddTags("MonitoringApplicationService");
            organizersRepository.AddTags("FlightRepository");



            ComponentView userComponentView = viewSet.CreateComponentView(userBoundedContext, "User Components", "User Component Diagram");
            userComponentView.PaperSize = PaperSize.A4_Landscape;
            
            userComponentView.Add(apiGateway);
            userComponentView.Add(webApplication);
            userComponentView.Add(userContextDatabase);
            userComponentView.AddAllComponents();

            //Event bounded context

            Component eventController =  eventBoundedContext.AddComponent("Event Controller", "Controller de los eventos", "Spring Boot REST Controller");
            Component eventService =     eventBoundedContext.AddComponent("Event Service", "Provee métodos para manejar a los eventos", "Spring Component");
            Component eventRepository =  eventBoundedContext.AddComponent("Event Repository", "Provee métodos para la persistencia de datos de los eventos", "Spring Component");

            Component placeController =  eventBoundedContext.AddComponent("Place Controller", "Controller de los lugares", "Spring Boot REST Controller");
            Component placeService =     eventBoundedContext.AddComponent("Place Service", "Provee métodos para manejar a los lugares", "Spring Component");
            Component placeRepository =  eventBoundedContext.AddComponent("Place Repository", "Provee métodos para la persistencia de datos de los lugares", "Spring Component");

            Component tagController =  eventBoundedContext.AddComponent("Tag Controller", "Controller de los tags", "Spring Boot REST Controller");
            Component tagService =     eventBoundedContext.AddComponent("Tag Service", "Provee métodos para manejar a los tags", "Spring Component");
            Component tagRepository =  eventBoundedContext.AddComponent("Tag Repository", "Provee métodos para la persistencia de datos de los tags", "Spring Component");

            Component itineraryController = eventBoundedContext.AddComponent("Itinerary Controller", "Controller de los itinerarios", "Spring Boot REST Controller");
            Component itineraryService = eventBoundedContext.AddComponent("Itinerary Service", "Provee métodos para manejar a los itinerarios", "Spring Component");
            Component itineraryRepository = eventBoundedContext.AddComponent("Itinerary Repository", "Provee métodos para la persistencia de datos de los itinerarios", "Spring Component");

            apiGateway.Uses(eventController, "", "JSON/HTTPS");
            apiGateway.Uses(placeController, "", "JSON/HTTPS");
            apiGateway.Uses(tagController, "", "JSON/HTTPS");
            apiGateway.Uses(itineraryController, "", "JSON/HTTPS");

            eventController.Uses(eventService, "Llama a los metodos del service");
            eventService.Uses(eventRepository, "Llama a los metodos del repository");
            /*ojo*/
            eventService.Uses(emailSystem, "Administra el envio de correos al usuario organizador");
            eventRepository.Uses(eventContextDatabase, "Escribe y lee en");

            placeController.Uses(placeService, "Llama a los metodos del service");
            placeService.Uses(placeRepository, "Llama a los metodos del repository");
            placeRepository.Uses(eventContextDatabase, "Escribe y lee en");

            tagController.Uses(tagService, "Llama a los metodos del service");
            tagService.Uses(tagRepository, "Llama a los metodos del repository");
            tagRepository.Uses(eventContextDatabase, "Escribe y lee en");

            itineraryController.Uses(itineraryService, "Llama a los metodos del service");
            itineraryService.Uses(itineraryRepository, "Llama a los metodos del repository");
            itineraryRepository.Uses(eventContextDatabase, "Escribe y lee en");

            eventController.AddTags("MonitoringController");
            eventService.AddTags("MonitoringApplicationService");
            eventRepository.AddTags("FlightRepository");

            placeController.AddTags("MonitoringController");
            placeService.AddTags("MonitoringApplicationService");
            placeRepository.AddTags("FlightRepository");

            tagController.AddTags("MonitoringController");
            tagService.AddTags("MonitoringApplicationService");
            tagRepository.AddTags("FlightRepository");

            itineraryController.AddTags("MonitoringController");
            itineraryService.AddTags("MonitoringApplicationService");
            itineraryRepository.AddTags("FlightRepository");

            ComponentView eventComponentView = viewSet.CreateComponentView(eventBoundedContext, "Event Components", "Event Component Diagram");
            eventComponentView.PaperSize = PaperSize.A4_Landscape;
            
            eventComponentView.Add(apiGateway);
            eventComponentView.Add(webApplication);
            eventComponentView.Add(eventContextDatabase);
            eventComponentView.AddAllComponents();

            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}