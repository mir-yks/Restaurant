CREATE DATABASE  IF NOT EXISTS `restaurant` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `restaurant`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: restaurant
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `CategoryDish`
--

DROP TABLE IF EXISTS `CategoryDish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CategoryDish` (
  `CategoryDishId` int NOT NULL AUTO_INCREMENT,
  `CategoryDishName` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoryDishId`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `CategoryDish`
--

LOCK TABLES `CategoryDish` WRITE;
/*!40000 ALTER TABLE `CategoryDish` DISABLE KEYS */;
INSERT INTO `CategoryDish` VALUES (1,'Салаты'),(2,'Супы'),(3,'Горячие закуски'),(4,'Холодные закуски'),(5,'Паста'),(6,'Пицца'),(7,'Мясные блюда'),(8,'Рыбные блюда'),(9,'Гарниры'),(10,'Десерты'),(11,'Соусы'),(12,'Хлеб и выпечка'),(13,'Сырное ассорти'),(14,'Напитки безалкогольные'),(15,'Вино и алкоголь');
/*!40000 ALTER TABLE `CategoryDish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Client`
--

DROP TABLE IF EXISTS `Client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Client` (
  `ClientId` int NOT NULL AUTO_INCREMENT,
  `ClientFIO` varchar(150) NOT NULL,
  `ClientPhone` varchar(11) NOT NULL,
  PRIMARY KEY (`ClientId`)
) ENGINE=InnoDB AUTO_INCREMENT=101 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Client`
--

LOCK TABLES `Client` WRITE;
/*!40000 ALTER TABLE `Client` DISABLE KEYS */;
INSERT INTO `Client` VALUES (1,'Иванов Алексей Петрович','79001234501'),(2,'Сидорова Мария Александровна','79001234502'),(3,'Павлов Иван Игоревич','79001234503'),(4,'Смирнова Ольга Сергеевна','79001234504'),(5,'Фёдоров Дмитрий Николаевич','79001234505'),(6,'Кузнецова Екатерина Викторовна','79001234506'),(7,'Морозов Андрей Андреевич','79001234507'),(8,'Волкова Светлана Дмитриевна','79001234508'),(9,'Егоров Павел Олегович','79001234509'),(10,'Михайлова Анна Павловна','79001234510'),(11,'Петров Николай Александрович','79001234511'),(12,'Соколова Юлия Ивановна','79001234512'),(13,'Козлов Виктор Никитич','79001234513'),(14,'Новикова Ирина Фёдоровна','79001234514'),(15,'Сергеев Сергей Владимирович','79001234515'),(16,'Зайцева Анастасия Сергеевна','79001234516'),(17,'Попов Максим Петрович','79001234517'),(18,'Орлова Татьяна Игоревна','79001234518'),(19,'Лебедев Олег Александрович','79001234519'),(20,'Семенова Елена Алексеевна','79001234520'),(21,'Виноградов Филипп Михайлович','79001234521'),(22,'Беляева Дарья Владимировна','79001234522'),(23,'Гусев Константин Иванович','79001234523'),(24,'Крылова Наталья Константиновна','79001234524'),(25,'Мельников Владимир Сергеевич','79001234525'),(26,'Кудрявцева Ксения Дмитриевна','79001234526'),(27,'Соловьёв Роман Викторович','79001234527'),(28,'Васильева Полина Александровна','79001234528'),(29,'Голубев Артур Олегович','79001234529'),(30,'Фомина Алёна Егоровна','79001234530'),(31,'Дюпон Жан Луиович','79001234531'),(32,'Морис Клер Антуановна','79001234532'),(33,'Лоран Луи Жанович','79001234533'),(34,'Лефевр Мари Огюстовна','79001234534'),(35,'Мартен Пьер Жоржевич','79001234535'),(36,'Шмидт Анна Карловна','79001234536'),(37,'Бауэр Фриц Гансович','79001234537'),(38,'Фогель Лена Фридриховна','79001234538'),(39,'Мюллер Томас Иоганнович','79001234539'),(40,'России Лаура Луиджиевна','79001234540'),(41,'Бьянки Марко Франческович','79001234541'),(42,'Риччи София Анжеловна','79001234542'),(43,'Феррари Давиде Карлович','79001234543'),(44,'Сантос Карина Робертовна','79001234544'),(45,'Гарсия Мигель Альбертович','79001234545'),(46,'Фернандес Изабель Рафаэлевна','79001234546'),(47,'Лопес Антонио Мануэлевич','79001234547'),(48,'Рамирес Хосе Давидович','79001234548'),(49,'Мартинес Камилла Карлосовна','79001234549'),(50,'Шнайдер Ганс Фридрихович','79001234550');
/*!40000 ALTER TABLE `Client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `MenuDish`
--

DROP TABLE IF EXISTS `MenuDish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `MenuDish` (
  `DishId` int NOT NULL AUTO_INCREMENT,
  `DishName` varchar(100) NOT NULL,
  `DishDescription` text NOT NULL,
  `DishPrice` decimal(10,2) NOT NULL,
  `DishCategory` int NOT NULL,
  `OffersDish` int DEFAULT NULL,
  `DishPhoto` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`DishId`),
  KEY `DishCategory` (`DishCategory`),
  KEY `OffersDish` (`OffersDish`),
  CONSTRAINT `menudish_ibfk_1` FOREIGN KEY (`DishCategory`) REFERENCES `CategoryDish` (`CategoryDishId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `menudish_ibfk_2` FOREIGN KEY (`OffersDish`) REFERENCES `OffersDish` (`OffersDishId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `MenuDish`
--

LOCK TABLES `MenuDish` WRITE;
/*!40000 ALTER TABLE `MenuDish` DISABLE KEYS */;
INSERT INTO `MenuDish` VALUES (1,'Цезарь с курицей','Классический салат с куриной грудкой, сухариками и соусом цезарь',450.00,1,NULL,NULL),(2,'Греческий салат','Огурцы, помидоры, маслины, фета, оливковое масло',390.00,1,4,NULL),(3,'Салат Нисуаз','Французский салат с тунцом, яйцом и фасолью',520.00,1,NULL,NULL),(4,'Минестроне','Итальянский овощной суп с пастой и травами',350.00,2,1,NULL),(5,'Французский луковый суп','Запечённый под сырной корочкой',420.00,2,NULL,NULL),(6,'Борщ европейский','Свекольный суп с говядиной и сметаной',380.00,2,NULL,NULL),(7,'Жульен с грибами','Запечённые шампиньоны в сливочном соусе',360.00,3,NULL,NULL),(8,'Креветки в чесночном соусе','Обжаренные тигровые креветки с чесноком и петрушкой',890.00,3,2,NULL),(9,'Мидии по-провански','Мидии в белом вине с травами',950.00,3,1,NULL),(10,'Карпаччо из говядины','Тонкие ломтики говядины с соусом песто',720.00,4,NULL,NULL),(11,'Тар-тар из лосося','Свежий лосось с авокадо и лаймом',860.00,4,4,NULL),(12,'Антипасти ассорти','Овощи гриль, оливки, прошутто',670.00,4,NULL,NULL),(13,'Спагетти Карбонара','Спагетти с беконом, сливками и пармезаном',490.00,5,2,NULL),(14,'Пенне Арабьята','Острая паста с томатным соусом',440.00,5,NULL,NULL),(15,'Лазанья Болоньезе','Классическая лазанья с мясным соусом',580.00,5,1,NULL),(16,'Фетучини Альфредо','Сливочная паста с курицей',510.00,5,NULL,NULL),(17,'Пицца Маргарита','Тесто, соус, моцарелла, базилик',490.00,6,NULL,NULL),(18,'Пицца Пепперони','Острая колбаса пепперони, сыр моцарелла',560.00,6,2,NULL),(19,'Пицца Четыре сыра','Моцарелла, горгонзола, пармезан, эмменталь',640.00,6,4,NULL),(20,'Пицца Капричоза','Ветчина, грибы, артишоки, оливки',670.00,6,NULL,NULL),(21,'Стейк Рибай','Мраморная говядина, прожарка на выбор',1650.00,7,1,NULL),(22,'Шницель Венский','Традиционный шницель из телятины',890.00,7,NULL,NULL),(23,'Котлета по-киевски','Курица с маслом и зеленью внутри',540.00,7,2,NULL),(24,'Утка Конфи','Французское блюдо из утки, томлённой в собственном жире',1450.00,7,NULL,NULL),(25,'Форель на гриле','Форель с лимоном и травами',890.00,8,1,NULL),(26,'Филе трески в сливочном соусе','Подаётся с овощами',760.00,8,NULL,NULL),(27,'Палтус под соусом терияки','Сезонное предложение',1100.00,8,1,NULL),(28,'Кальмары гриль','Кальмары с чесночным соусом',680.00,8,NULL,NULL),(29,'Картофель по-деревенски','Запечённый картофель с чесноком и укропом',250.00,9,NULL,NULL),(30,'Овощи гриль','Кабачки, баклажаны, перец, томаты',320.00,9,4,NULL),(31,'Ризотто с грибами','Итальянский рис с белыми грибами',520.00,9,NULL,NULL),(32,'Тирамису','Классический итальянский десерт',390.00,10,2,NULL),(33,'Чизкейк Нью-Йорк','Сливочный чизкейк с клубничным соусом',420.00,10,NULL,NULL),(34,'Панна Котта','Итальянский сливочный десерт',370.00,10,NULL,NULL),(35,'Крем-брюле','Французский десерт с карамельной корочкой',450.00,10,1,NULL),(36,'Штрудель яблочный','С яблоками и корицей, подаётся с мороженым',410.00,10,NULL,NULL),(37,'Соус Песто','Традиционный соус из базилика и орехов',120.00,11,NULL,NULL),(38,'Соус Сальса','Острый томатный соус',100.00,11,NULL,NULL),(39,'Соус Тартар','Майонез, корнишоны, зелень',90.00,11,NULL,NULL),(40,'Чиабатта','Итальянский пшеничный хлеб',80.00,12,NULL,NULL),(41,'Фокачча с розмарином','Тонкая лепёшка с оливковым маслом',110.00,12,4,NULL),(42,'Булочки с чесноком','Свежая выпечка к супам и закускам',90.00,12,NULL,NULL),(43,'Ассорти из итальянских сыров','Пармезан, горгонзола, моцарелла',560.00,13,3,NULL),(44,'Ассорти из французских сыров','Камамбер, бри, рокфор',620.00,13,NULL,NULL),(45,'Эспрессо','Кофе крепкий',150.00,14,NULL,NULL),(46,'Капучино','Кофе с молочной пеной',190.00,14,NULL,NULL),(47,'Апельсиновый фреш','Свежевыжатый сок',250.00,14,1,NULL),(48,'Минеральная вода','Газированная/негазированная',120.00,14,NULL,NULL),(49,'Шардоне','Белое сухое вино, Франция',950.00,15,NULL,NULL),(50,'Кьянти','Красное сухое вино, Италия',1100.00,15,5,NULL),(51,'Просекко','Игристое вино, Италия',1250.00,15,5,NULL),(52,'Сангрия','Испанский напиток с фруктами и вином',890.00,15,3,NULL);
/*!40000 ALTER TABLE `MenuDish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `OffersDish`
--

DROP TABLE IF EXISTS `OffersDish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OffersDish` (
  `OffersDishId` int NOT NULL AUTO_INCREMENT,
  `OffersDishName` varchar(100) NOT NULL,
  `OffersDishDicsount` int NOT NULL,
  PRIMARY KEY (`OffersDishId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `OffersDish`
--

LOCK TABLES `OffersDish` WRITE;
/*!40000 ALTER TABLE `OffersDish` DISABLE KEYS */;
INSERT INTO `OffersDish` VALUES (1,'Сезонное',5),(2,'Комбо',5),(3,'Праздничные акции',10),(4,'Диетическое',15),(5,'Тематический набор',10);
/*!40000 ALTER TABLE `OffersDish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Order`
--

DROP TABLE IF EXISTS `Order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Order` (
  `OrderId` int NOT NULL AUTO_INCREMENT,
  `WorkerId` int NOT NULL,
  `ClientId` int DEFAULT NULL,
  `TableId` int NOT NULL,
  `OrderDate` datetime NOT NULL,
  `OrderPrice` decimal(10,2) NOT NULL,
  `OrderStatus` enum('Готов','На кухне','В обработке','Принят') NOT NULL,
  `OrderStatusPayment` enum('Оплачен','Не оплачен') NOT NULL,
  PRIMARY KEY (`OrderId`),
  KEY `order_ibfk_1` (`WorkerId`),
  KEY `order_ibfk_2` (`ClientId`),
  KEY `order_ibfk_3` (`TableId`),
  CONSTRAINT `order_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `Worker` (`WorkerId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `order_ibfk_2` FOREIGN KEY (`ClientId`) REFERENCES `Client` (`ClientId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `order_ibfk_3` FOREIGN KEY (`TableId`) REFERENCES `Tables` (`TablesId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Order`
--

LOCK TABLES `Order` WRITE;
/*!40000 ALTER TABLE `Order` DISABLE KEYS */;
INSERT INTO `Order` VALUES (1,16,1,1,'2024-01-15 18:45:00',2450.00,'Принят','Не оплачен'),(2,17,2,2,'2024-02-10 19:10:00',3200.00,'В обработке','Оплачен'),(3,18,3,3,'2024-03-05 13:30:00',1800.00,'На кухне','Оплачен'),(4,19,4,4,'2024-03-22 20:15:00',5600.00,'Готов','Оплачен'),(5,20,5,5,'2024-04-01 17:20:00',4300.00,'Принят','Не оплачен'),(6,21,6,6,'2024-04-18 12:40:00',1200.00,'В обработке','Оплачен'),(7,22,7,7,'2024-05-09 21:05:00',2750.00,'На кухне','Оплачен'),(8,23,8,8,'2024-05-25 19:55:00',3500.00,'Готов','Оплачен'),(9,24,9,9,'2024-06-03 18:25:00',2200.00,'Принят','Не оплачен'),(10,25,10,10,'2024-06-21 20:45:00',4000.00,'В обработке','Оплачен'),(11,26,11,11,'2024-07-12 13:15:00',1900.00,'На кухне','Оплачен'),(12,27,12,12,'2024-07-28 14:10:00',2850.00,'Готов','Не оплачен'),(13,28,13,13,'2024-08-05 15:30:00',3600.00,'Принят','Оплачен'),(14,29,14,14,'2024-08-19 18:00:00',4100.00,'В обработке','Не оплачен'),(15,30,15,15,'2024-09-02 20:35:00',5400.00,'На кухне','Оплачен'),(16,31,16,16,'2024-09-16 19:25:00',3000.00,'Готов','Оплачен'),(17,32,17,17,'2024-10-03 18:50:00',1950.00,'Принят','Не оплачен'),(18,33,18,18,'2024-10-21 21:15:00',6200.00,'В обработке','Оплачен'),(19,34,19,19,'2024-11-05 17:10:00',2700.00,'На кухне','Оплачен'),(20,35,20,20,'2024-11-18 13:35:00',3550.00,'Готов','Не оплачен'),(21,36,21,21,'2024-12-02 20:05:00',4800.00,'Принят','Оплачен'),(22,37,22,22,'2024-12-15 19:40:00',2100.00,'В обработке','Оплачен'),(23,38,23,23,'2024-12-29 18:20:00',3700.00,'На кухне','Не оплачен'),(24,39,24,24,'2025-01-11 13:50:00',2950.00,'Готов','Оплачен'),(25,40,25,25,'2025-01-25 17:55:00',4600.00,'Принят','Не оплачен'),(26,41,26,26,'2025-02-08 20:10:00',2800.00,'В обработке','Оплачен'),(27,42,27,27,'2025-02-22 14:25:00',3300.00,'На кухне','Оплачен'),(28,43,28,28,'2025-03-06 19:05:00',5100.00,'Готов','Оплачен'),(29,44,29,29,'2025-03-20 21:30:00',1850.00,'Принят','Не оплачен'),(30,45,30,30,'2025-04-04 18:15:00',4200.00,'В обработке','Оплачен'),(31,46,31,31,'2025-04-18 13:40:00',2450.00,'На кухне','Оплачен'),(32,47,32,32,'2025-05-02 20:25:00',3750.00,'Готов','Не оплачен'),(33,48,33,33,'2025-05-16 19:45:00',2950.00,'Принят','Оплачен'),(34,49,34,34,'2025-05-30 18:35:00',5600.00,'В обработке','Не оплачен'),(35,50,35,35,'2025-06-13 17:55:00',2200.00,'На кухне','Оплачен'),(36,36,36,36,'2025-06-27 21:05:00',3300.00,'Готов','Оплачен'),(37,37,37,37,'2025-07-11 20:30:00',4800.00,'Принят','Не оплачен'),(38,38,38,38,'2025-07-25 19:20:00',2600.00,'В обработке','Оплачен'),(39,39,39,39,'2025-08-08 18:00:00',3900.00,'На кухне','Оплачен'),(40,40,40,40,'2025-08-22 17:40:00',4150.00,'Готов','Оплачен'),(41,41,41,41,'2025-09-05 21:00:00',1850.00,'Принят','Не оплачен'),(42,42,42,42,'2025-09-19 20:15:00',2800.00,'В обработке','Оплачен'),(43,43,43,43,'2025-10-03 19:35:00',3500.00,'На кухне','Не оплачен'),(44,44,44,44,'2025-10-17 18:50:00',4700.00,'Готов','Оплачен'),(45,45,45,45,'2025-10-31 13:25:00',3250.00,'Принят','Оплачен'),(46,46,46,46,'2025-11-14 20:05:00',5600.00,'В обработке','Не оплачен'),(47,47,47,47,'2025-11-28 19:10:00',2750.00,'На кухне','Оплачен'),(48,48,48,48,'2025-12-12 18:15:00',3900.00,'Готов','Оплачен'),(49,49,49,49,'2025-12-20 21:20:00',4400.00,'Принят','Не оплачен'),(50,50,50,50,'2025-12-24 19:55:00',3600.00,'В обработке','Оплачен');
/*!40000 ALTER TABLE `Order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `OrderItems`
--

DROP TABLE IF EXISTS `OrderItems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OrderItems` (
  `OrderId` int NOT NULL,
  `DishId` int NOT NULL,
  `DishCount` int NOT NULL,
  PRIMARY KEY (`OrderId`,`DishId`),
  KEY `orderitems_ibfk_2` (`DishId`),
  CONSTRAINT `orderitems_ibfk_1` FOREIGN KEY (`OrderId`) REFERENCES `Order` (`OrderId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `orderitems_ibfk_2` FOREIGN KEY (`DishId`) REFERENCES `MenuDish` (`DishId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `OrderItems`
--

LOCK TABLES `OrderItems` WRITE;
/*!40000 ALTER TABLE `OrderItems` DISABLE KEYS */;
INSERT INTO `OrderItems` VALUES (1,1,2),(1,15,1),(1,28,2),(2,3,1),(2,20,2),(2,45,1),(3,5,2),(3,12,1),(3,33,1),(4,8,1),(4,26,2),(4,39,2),(5,9,1),(5,17,2),(5,44,1),(6,2,2),(6,18,1),(6,36,1),(7,6,1),(7,24,2),(7,47,1),(8,4,1),(8,30,2),(8,41,2),(9,7,2),(9,22,1),(9,40,1),(10,10,1),(10,25,2),(10,48,2),(11,13,1),(11,32,2),(11,46,1),(12,11,2),(12,19,1),(12,42,2),(13,14,1),(13,21,2),(13,37,1),(14,16,2),(14,27,1),(14,50,2),(15,23,1),(15,31,2),(15,35,1),(16,29,2),(16,38,1),(16,43,2),(17,12,1),(17,24,2),(17,36,1),(18,8,2),(18,33,1),(18,49,1),(19,2,1),(19,18,2),(19,41,2),(20,5,2),(20,20,1),(20,47,1),(21,7,1),(21,25,2),(21,39,1),(22,9,2),(22,28,1),(22,44,2),(23,4,1),(23,21,2),(23,34,1),(24,3,2),(24,15,1),(24,46,2),(25,10,1),(25,26,2),(25,48,1),(26,6,1),(26,29,2),(26,42,2),(27,11,2),(27,23,1),(27,36,1),(28,1,1),(28,17,2),(28,37,2),(29,13,2),(29,32,1),(29,40,1),(30,8,2),(30,19,1),(30,49,1),(31,2,1),(31,24,2),(31,35,2),(32,5,2),(32,28,1),(32,43,1),(33,14,1),(33,22,2),(33,38,1),(34,6,2),(34,18,1),(34,44,2),(35,7,1),(35,20,2),(35,45,1),(36,9,2),(36,30,1),(36,50,2),(37,11,2),(37,16,1),(37,39,1),(38,12,1),(38,23,2),(38,42,2),(39,4,2),(39,27,1),(39,48,1),(40,10,1),(40,21,2),(40,36,1),(41,8,1),(41,18,2),(41,35,1),(42,3,2),(42,14,1),(42,46,2),(43,6,2),(43,19,1),(43,40,1),(44,9,2),(44,22,1),(44,47,2),(45,1,1),(45,26,2),(45,34,2),(46,7,1),(46,25,2),(46,41,1),(47,2,2),(47,15,1),(47,39,1),(48,5,1),(48,28,2),(48,43,2),(49,13,2),(49,20,1),(49,50,1),(50,6,2),(50,18,1),(50,37,2);
/*!40000 ALTER TABLE `OrderItems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Role`
--

DROP TABLE IF EXISTS `Role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Role` (
  `RoleId` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(20) NOT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Role`
--

LOCK TABLES `Role` WRITE;
/*!40000 ALTER TABLE `Role` DISABLE KEYS */;
INSERT INTO `Role` VALUES (1,'Администратор'),(2,'Менеджер'),(3,'Официант');
/*!40000 ALTER TABLE `Role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Tables`
--

DROP TABLE IF EXISTS `Tables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Tables` (
  `TablesId` int NOT NULL AUTO_INCREMENT,
  `TablesCountPlace` int NOT NULL,
  `TablesStatus` enum('Свободен','Забронирован','Занят') NOT NULL,
  PRIMARY KEY (`TablesId`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Tables`
--

LOCK TABLES `Tables` WRITE;
/*!40000 ALTER TABLE `Tables` DISABLE KEYS */;
INSERT INTO `Tables` VALUES (1,2,'Свободен'),(2,4,'Забронирован'),(3,6,'Занят'),(4,2,'Свободен'),(5,8,'Забронирован'),(6,4,'Занят'),(7,2,'Свободен'),(8,6,'Забронирован'),(9,4,'Свободен'),(10,8,'Занят'),(11,2,'Свободен'),(12,4,'Забронирован'),(13,6,'Свободен'),(14,8,'Занят'),(15,2,'Свободен'),(16,4,'Забронирован'),(17,6,'Занят'),(18,2,'Свободен'),(19,8,'Свободен'),(20,4,'Занят'),(21,6,'Забронирован'),(22,2,'Свободен'),(23,4,'Занят'),(24,8,'Свободен'),(25,2,'Забронирован'),(26,6,'Занят'),(27,4,'Свободен'),(28,8,'Забронирован'),(29,2,'Занят'),(30,6,'Свободен'),(31,4,'Забронирован'),(32,8,'Занят'),(33,2,'Свободен'),(34,4,'Забронирован'),(35,6,'Занят'),(36,8,'Свободен'),(37,2,'Забронирован'),(38,4,'Свободен'),(39,6,'Занят'),(40,8,'Забронирован'),(41,2,'Свободен'),(42,4,'Занят'),(43,6,'Свободен'),(44,8,'Забронирован'),(45,2,'Занят'),(46,4,'Свободен'),(47,6,'Забронирован'),(48,8,'Занят'),(49,2,'Свободен'),(50,4,'Забронирован');
/*!40000 ALTER TABLE `Tables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Worker`
--

DROP TABLE IF EXISTS `Worker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Worker` (
  `WorkerId` int NOT NULL AUTO_INCREMENT,
  `WorkerFIO` varchar(150) NOT NULL,
  `WorkerLogin` varchar(100) NOT NULL,
  `WorkerPassword` varchar(200) NOT NULL,
  `WorkerPhone` varchar(11) NOT NULL,
  `WorkerEmail` varchar(100) NOT NULL,
  `WorkerBirthday` date NOT NULL,
  `WorkerDateEmployment` date NOT NULL,
  `WorkerAddress` text NOT NULL,
  `WorkerRole` int NOT NULL,
  PRIMARY KEY (`WorkerId`),
  KEY `worker_ibfk_1` (`WorkerRole`),
  CONSTRAINT `worker_ibfk_1` FOREIGN KEY (`WorkerRole`) REFERENCES `Role` (`RoleId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Worker`
--

LOCK TABLES `Worker` WRITE;
/*!40000 ALTER TABLE `Worker` DISABLE KEYS */;
INSERT INTO `Worker` VALUES (1,'Сидоров Иван Петрович','admin1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000001','admin1@resto.ru','1980-05-12','2021-03-01','Москва, ул. Тверская, д.1',1),(2,'Фролова Мария Александровна','admin2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000002','admin2@resto.ru','1985-07-24','2020-08-15','Москва, ул. Арбат, д.15',1),(3,'Кузнецов Олег Игоревич','admin3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000003','admin3@resto.ru','1979-12-30','2022-01-10','Москва, ул. Сретенка, д.3',1),(4,'Морозова Екатерина Владимировна','admin4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000004','admin4@resto.ru','1988-09-09','2023-06-05','Москва, ул. Пречистенка, д.7',1),(5,'Волков Антон Сергеевич','admin5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000005','admin5@resto.ru','1982-11-20','2024-02-18','Москва, ул. Петровка, д.22',1),(6,'Иванов Павел Константинович','manager1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000006','manager1@resto.ru','1990-03-14','2021-05-11','Москва, ул. Маросейка, д.10',2),(7,'Егорова Светлана Михайловна','manager2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000007','manager2@resto.ru','1992-06-28','2020-09-01','Москва, ул. Никитская, д.12',2),(8,'Попов Дмитрий Олегович','manager3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000008','manager3@resto.ru','1987-01-19','2021-12-20','Москва, ул. Остоженка, д.19',2),(9,'Соколова Ирина Фёдоровна','manager4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000009','manager4@resto.ru','1995-04-05','2022-02-01','Москва, ул. Покровка, д.33',2),(10,'Зайцев Владислав Романович','manager5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000010','manager5@resto.ru','1989-08-08','2023-04-17','Москва, ул. Шаболовка, д.25',2),(11,'Орлова Анастасия Васильевна','manager6','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000011','manager6@resto.ru','1993-12-01','2021-07-21','Москва, ул. Кузнецкий Мост, д.5',2),(12,'Лебедев Сергей Андреевич','manager7','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000012','manager7@resto.ru','1991-09-14','2022-11-11','Москва, ул. Варварка, д.17',2),(13,'Крылова Ольга Дмитриевна','manager8','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000013','manager8@resto.ru','1986-07-07','2020-10-09','Москва, ул. Б. Никитская, д.14',2),(14,'Фомин Никита Евгеньевич','manager9','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000014','manager9@resto.ru','1994-02-22','2021-01-30','Москва, ул. Б. Ордынка, д.27',2),(15,'Васильева Татьяна Ивановна','manager10','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000015','manager10@resto.ru','1988-05-18','2024-06-02','Москва, ул. Мясницкая, д.16',2),(16,'Смирнов Алексей Петрович','waiter1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000016','waiter1@resto.ru','1998-01-10','2022-05-01','Москва, ул. Лесная, д.3',3),(17,'Новикова Юлия Александровна','waiter2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000017','waiter2@resto.ru','1997-03-15','2023-02-12','Москва, ул. Садовая, д.14',3),(18,'Сергеев Григорий Дмитриевич','waiter3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000018','waiter3@resto.ru','1996-07-20','2021-09-05','Москва, ул. Таганская, д.8',3),(19,'Белова Ксения Фёдоровна','waiter4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000019','waiter4@resto.ru','1999-11-11','2024-03-17','Москва, ул. Якиманка, д.19',3),(20,'Козлов Иван Олегович','waiter5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000020','waiter5@resto.ru','1995-09-01','2020-07-23','Москва, ул. Сретенка, д.21',3),(21,'Медведева Алина Романовна','waiter6','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000021','waiter6@resto.ru','2000-06-14','2022-08-29','Москва, ул. Цветной бульвар, д.9',3),(22,'Борисов Николай Александрович','waiter7','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000022','waiter7@resto.ru','1994-10-10','2021-04-11','Москва, ул. Новослободская, д.11',3),(23,'Григорьева Вера Игоревна','waiter8','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000023','waiter8@resto.ru','1997-12-25','2023-05-19','Москва, ул. Большая Спасская, д.7',3),(24,'Алексеев Роман Михайлович','waiter9','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000024','waiter9@resto.ru','1993-05-30','2022-01-28','Москва, ул. Малая Дмитровка, д.4',3),(25,'Ефимова Полина Андреевна','waiter10','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000025','waiter10@resto.ru','1998-02-14','2024-07-12','Москва, ул. Земляной Вал, д.13',3),(26,'Никитин Артём Ильич','waiter11','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000026','waiter11@resto.ru','1996-06-07','2021-03-22','Москва, ул. Арбат, д.45',3),(27,'Кузьмина Елена Павловна','waiter12','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000027','waiter12@resto.ru','1997-01-30','2022-07-15','Москва, ул. Волхонка, д.6',3),(28,'Макаров Фёдор Игоревич','waiter13','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000028','waiter13@resto.ru','1995-11-21','2020-11-09','Москва, ул. Ленинский пр-т, д.70',3),(29,'Дмитриева Карина Алексеевна','waiter14','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000029','waiter14@resto.ru','1998-08-18','2023-09-12','Москва, ул. Профсоюзная, д.12',3),(30,'Поляков Степан Олегович','waiter15','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000030','waiter15@resto.ru','1999-12-04','2021-05-07','Москва, ул. Вавилова, д.8',3),(31,'Захарова Марина Ильинична','waiter16','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000031','waiter16@resto.ru','1996-04-14','2022-02-20','Москва, ул. Шоссе Энтузиастов, д.15',3),(32,'Комаров Даниил Фёдорович','waiter17','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000032','waiter17@resto.ru','1995-07-09','2023-01-25','Москва, ул. Сущёвская, д.5',3),(33,'Богданова Людмила Сергеевна','waiter18','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000033','waiter18@resto.ru','1998-09-23','2021-06-30','Москва, ул. Новая Басманная, д.18',3),(34,'Савельев Игорь Викторович','waiter19','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000034','waiter19@resto.ru','1994-02-18','2020-08-10','Москва, ул. Кирова, д.19',3),(35,'Романова Дарья Анатольевна','waiter20','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000035','waiter20@resto.ru','1999-10-29','2022-12-12','Москва, ул. Лубянка, д.2',3),(36,'Миронов Валерий Андреевич','waiter21','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000036','waiter21@resto.ru','1997-05-11','2024-04-04','Москва, ул. Садово-Каретная, д.8',3),(37,'Тихонова Алёна Ивановна','waiter22','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000037','waiter22@resto.ru','1996-07-26','2021-09-19','Москва, ул. Малая Бронная, д.7',3),(38,'Гаврилов Виталий Степанович','waiter23','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000038','waiter23@resto.ru','1995-01-02','2023-03-23','Москва, ул. Остаповская, д.16',3),(39,'Федорова Ангелина Дмитриевна','waiter24','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000039','waiter24@resto.ru','1997-03-03','2022-05-05','Москва, ул. Рязанский пр-т, д.25',3),(40,'Соболев Михаил Николаевич','waiter25','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000040','waiter25@resto.ru','1998-11-15','2020-10-22','Москва, ул. Шоссейная, д.12',3),(41,'Карпова Оксана Артемовна','waiter26','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000041','waiter26@resto.ru','1999-12-28','2024-01-13','Москва, ул. Щепкина, д.20',3),(42,'Мельников Георгий Васильевич','waiter27','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000042','waiter27@resto.ru','1996-02-17','2021-08-30','Москва, ул. Бакунинская, д.23',3),(43,'Ковалева Лилия Евгеньевна','waiter28','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000043','waiter28@resto.ru','1997-06-19','2023-07-14','Москва, ул. Рочдельская, д.15',3),(44,'Васильев Станислав Тимурович','waiter29','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000044','waiter29@resto.ru','1995-10-08','2022-09-27','Москва, ул. Нижняя Красносельская, д.11',3),(45,'Голубева Виктория Аркадьевна','waiter30','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000045','waiter30@resto.ru','1998-04-12','2021-12-03','Москва, ул. Большая Черкизовская, д.18',3),(46,'Власов Арсений Григорьевич','waiter31','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000046','waiter31@resto.ru','1999-09-21','2024-02-08','Москва, ул. Дубининская, д.14',3),(47,'Сидорова Инна Никитична','waiter32','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000047','waiter32@resto.ru','1997-11-07','2020-06-16','Москва, ул. Зоологическая, д.6',3),(48,'Павлов Вадим Романович','waiter33','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000048','waiter33@resto.ru','1996-08-13','2022-04-28','Москва, ул. Малая Никитская, д.9',3),(49,'Цветкова Евгения Владимировна','waiter34','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000049','waiter34@resto.ru','1998-12-20','2023-08-16','Москва, ул. Фрунзенская, д.7',3),(50,'Игнатьев Константин Борисович','waiter35','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000050','waiter35@resto.ru','1995-03-02','2021-11-09','Москва, ул. Краснопрудная, д.5',3);
/*!40000 ALTER TABLE `Worker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'restaurant'
--

--
-- Dumping routines for database 'restaurant'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-10-02 21:09:10
