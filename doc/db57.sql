CREATE DATABASE  IF NOT EXISTS `db57` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `db57`;
-- MySQL dump 10.13  Distrib 9.5.0, for Win64 (x86_64)
--
-- Host: localhost    Database: db57
-- ------------------------------------------------------
-- Server version	9.5.0

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
-- Table structure for table `booking`
--

DROP TABLE IF EXISTS `booking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `booking` (
  `BookingId` int NOT NULL AUTO_INCREMENT,
  `ClientId` int NOT NULL,
  `BookingDate` datetime NOT NULL,
  `ClientsCount` int NOT NULL,
  `TableId` int NOT NULL,
  PRIMARY KEY (`BookingId`),
  KEY `client_fk_idx` (`ClientId`),
  KEY `table_fk_idx` (`TableId`),
  CONSTRAINT `client_fk` FOREIGN KEY (`ClientId`) REFERENCES `client` (`ClientId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `table_fk` FOREIGN KEY (`TableId`) REFERENCES `tables` (`TablesId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `booking`
--

LOCK TABLES `booking` WRITE;
/*!40000 ALTER TABLE `booking` DISABLE KEYS */;
/*!40000 ALTER TABLE `booking` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categorydish`
--

DROP TABLE IF EXISTS `categorydish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categorydish` (
  `CategoryDishId` int NOT NULL AUTO_INCREMENT,
  `CategoryDishName` varchar(50) NOT NULL,
  PRIMARY KEY (`CategoryDishId`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorydish`
--

LOCK TABLES `categorydish` WRITE;
/*!40000 ALTER TABLE `categorydish` DISABLE KEYS */;
INSERT INTO `categorydish` VALUES (1,'Салаты'),(2,'Супы'),(3,'Горячие закуски'),(4,'Холодные закуски'),(5,'Паста'),(6,'Пицца'),(7,'Мясные блюда'),(8,'Рыбные блюда'),(9,'Гарниры'),(10,'Десерты'),(11,'Соусы'),(12,'Хлеб и выпечка'),(13,'Сырное ассорти'),(14,'Напитки безалкогольные'),(15,'Вино и алкоголь');
/*!40000 ALTER TABLE `categorydish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `ClientId` int NOT NULL AUTO_INCREMENT,
  `ClientFIO` varchar(100) NOT NULL,
  `OriginalClientFIO` varchar(100) DEFAULT NULL,
  `ClientPhone` varchar(11) NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`ClientId`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Петров Алексей Петрович','Иванов Алексей Петрович','79001234501',1),(2,'Сидорова Мария Александровна',NULL,'79001234502',1),(3,'Павлов Иван Игоревич',NULL,'79001234503',1),(4,'Смирнова Ольга Сергеевна',NULL,'79001234504',1),(5,'Фёдоров Дмитрий Николаевич',NULL,'79001234505',1),(6,'Кузнецова Екатерина Викторовна',NULL,'79001234506',1),(7,'Морозов Андрей Андреевич',NULL,'79001234507',1),(8,'Волкова Светлана Дмитриевна',NULL,'79001234508',1),(9,'Егоров Павел Олегович',NULL,'79001234509',1),(10,'Михайлова Анна Павловна',NULL,'79001234510',1),(11,'Петров Николай Александрович',NULL,'79001234511',1),(12,'Соколова Юлия Ивановна',NULL,'79001234512',1),(13,'Козлов Виктор Никитич',NULL,'79001234513',1),(14,'Новикова Ирина Фёдоровна',NULL,'79001234514',1),(15,'Сергеев Сергей Владимирович',NULL,'79001234515',1),(16,'Зайцева Анастасия Сергеевна',NULL,'79001234516',1),(17,'Попов Максим Петрович',NULL,'79001234517',1),(18,'Орлова Татьяна Игоревна',NULL,'79001234518',1),(19,'Лебедев Олег Александрович',NULL,'79001234519',1),(20,'Семенова Елена Алексеевна',NULL,'79001234520',1),(21,'Виноградов Филипп Михайлович',NULL,'79001234521',1),(22,'Беляева Дарья Владимировна',NULL,'79001234522',1),(23,'Гусев Константин Иванович',NULL,'79001234523',1),(24,'Крылова Наталья Константиновна',NULL,'79001234524',1),(25,'Мельников Владимир Сергеевич',NULL,'79001234525',1),(26,'Кудрявцева Ксения Дмитриевна',NULL,'79001234526',1),(27,'Соловьёв Роман Викторович',NULL,'79001234527',1),(28,'Васильева Полина Александровна',NULL,'79001234528',1),(29,'Голубев Артур Олегович',NULL,'79001234529',1),(30,'Фомина Алёна Егоровна',NULL,'79001234530',1),(31,'Дюпон Жан Луиович',NULL,'79001234531',1),(32,'Морис Клер Антуановна',NULL,'79001234532',1),(33,'Лоран Луи Жанович',NULL,'79001234533',1),(34,'Лефевр Мари Огюстовна',NULL,'79001234534',1),(35,'Мартен Пьер Жоржевич',NULL,'79001234535',1),(36,'Шмидт Анна Карловна',NULL,'79001234536',1),(37,'Бауэр Фриц Гансович',NULL,'79001234537',1),(38,'Фогель Лена Фридриховна',NULL,'79001234538',1),(39,'Мюллер Томас Иоганнович',NULL,'79001234539',1),(40,'России Лаура Луиджиевна',NULL,'79001234540',1),(41,'Бьянки Марко Франческович',NULL,'79001234541',1),(42,'Риччи София Анжеловна',NULL,'79001234542',1),(43,'Феррари Давиде Карлович',NULL,'79001234543',1),(44,'Сантос Карина Робертовна',NULL,'79001234544',1),(45,'Гарсия Мигель Альбертович',NULL,'79001234545',1),(46,'Фернандес Изабель Рафаэлевна',NULL,'79001234546',1),(47,'Лопес Антонио Мануэлевич',NULL,'79001234547',1),(48,'Рамирес Хосе Давидович',NULL,'79001234548',1),(49,'Мартинес Камилла Карлосовна',NULL,'79001234549',1),(50,'Шнайдер Ганс Фридрихович',NULL,'79001234550',1);
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menudish`
--

DROP TABLE IF EXISTS `menudish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menudish` (
  `DishId` int NOT NULL AUTO_INCREMENT,
  `DishName` varchar(100) NOT NULL,
  `OriginalDishName` varchar(100) DEFAULT NULL,
  `DishDescription` text NOT NULL,
  `DishPrice` decimal(10,2) NOT NULL,
  `DishCategory` int NOT NULL,
  `OffersDish` int DEFAULT NULL,
  `DishPhoto` text,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`DishId`),
  KEY `DishCategory` (`DishCategory`),
  KEY `OffersDish` (`OffersDish`),
  CONSTRAINT `menudish_ibfk_1` FOREIGN KEY (`DishCategory`) REFERENCES `categorydish` (`CategoryDishId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `menudish_ibfk_2` FOREIGN KEY (`OffersDish`) REFERENCES `offersdish` (`OffersDishId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menudish`
--

LOCK TABLES `menudish` WRITE;
/*!40000 ALTER TABLE `menudish` DISABLE KEYS */;
INSERT INTO `menudish` VALUES (1,'Цезарь с курицей','Цезарь с курицей','Классический салат с куриной грудкой, сухариками и соусом цезарь',450.00,1,NULL,'198f99b13d3e6f187631bf5b37b7a2df9dc658fb2524cc66188ac2515da48b55',1),(2,'Греческий салат','Греческий салат','Огурцы, помидоры, маслины, фета, оливковое масло',400.00,1,4,'c3d956ca708ef765a39e8868e854c45488a92a4c82a74bc2b5ce0eed40316fb1',1),(3,'Салат Нисуаз',NULL,'Французский салат с тунцом, яйцом и фасолью',520.00,1,NULL,'34f31b12eb75344fa59b9844b2a2acbc4c004fe604d05f195e4fbf9ac4bd6e05',1),(4,'Минестроне',NULL,'Итальянский овощной суп с пастой и травами',350.00,2,1,'8b3c3f024fad50ba1b07555c98ea919151dfbb81ca6a227be21305c68853392f',1),(5,'Французский луковый суп',NULL,'Запечённый под сырной корочкой',420.00,2,NULL,'e385fcaef25cc61e954ec9be3a723fc02308921a550c9a9858fdb324c5376d44',1),(6,'Борщ европейский',NULL,'Свекольный суп с говядиной и сметаной',380.00,2,NULL,'aa8b5ff053fd076a8fb603ae0fe80eb434eff6f738f0c6502fd803390175593d',1),(7,'Жульен с грибами',NULL,'Запечённые шампиньоны в сливочном соусе',360.00,3,NULL,'c40224ab8a03a453d4c5e75f03e4e9049d282e3388891342abd20cc03c08d0a3',1),(8,'Креветки в чесночном соусе',NULL,'Обжаренные тигровые креветки с чесноком и петрушкой',890.00,3,2,'a5dc2fa473d08181b5a1e1ce24b39b0c65d84f08787e67f9359d5435b87d6c8e',1),(9,'Мидии по-провански',NULL,'Мидии в белом вине с травами',950.00,3,1,'143e1377213d59c5bd704c21ca533208bfe621113598ea113bb2f402426b3bd6',1),(10,'Карпаччо из говядины',NULL,'Тонкие ломтики говядины с соусом песто',720.00,4,NULL,'1156d4182d813c435964fa32de8243bcd7303e2af9ad2e6e7e6ddce4ed1e7d39',1),(11,'Тар-тар из лосося',NULL,'Свежий лосось с авокадо и лаймом',860.00,4,4,'51496972d4710d1a89e21c3390513aa7b5d1b305e2d88e7e6080cc5ca9e119a2',1),(12,'Антипасти ассорти',NULL,'Овощи гриль, оливки, прошутто',670.00,4,NULL,'c213d2623e49832d57e89f106dcba1758c358c22bad484a5304a37f42c2637c1',1),(13,'Спагетти Карбонара',NULL,'Спагетти с беконом, сливками и пармезаном',490.00,5,2,'0b8c596eecd544edec89e4fe205e6bdffcd7b5606d383fd89634e06544016fd3',1),(14,'Пенне Арабьята',NULL,'Острая паста с томатным соусом',440.00,5,NULL,'d80aa85fd2c194c01e4f3a551b0001e33e47e7dab87297c257ee5043843ce8cd',1),(15,'Лазанья Болоньезе',NULL,'Классическая лазанья с мясным соусом',580.00,5,1,'1ed5b3e7540d31a7ea1734f393f022da36b5c1b0d98e0f9a29fd0fbd0a95c2f8',1),(16,'Фетучини Альфредо',NULL,'Сливочная паста с курицей',510.00,5,NULL,'fe2f12210f53d9c244e584d9a89c4b4c8aef3a67b2c8831eda88fdfc24757177',1),(17,'Пицца Маргарита',NULL,'Тесто, соус, моцарелла, базилик',490.00,6,NULL,'48ca3ad9d673953c7510c030140246b6071ac60233c5981f53a129f1dd8f91b4',1),(18,'Пицца Пепперони',NULL,'Острая колбаса пепперони, сыр моцарелла',560.00,6,2,'6d87f9d30aef61561fb48290f628aaa413c8228187e0c1806c987c7670aff304',1),(19,'Пицца Четыре сыра',NULL,'Моцарелла, горгонзола, пармезан, эмменталь',640.00,6,4,'4781b086b10211206ab0d874259a6311f8e843d5f1831ae84c2580bc48a58c38',1),(20,'Пицца Капричоза',NULL,'Ветчина, грибы, артишоки, оливки',670.00,6,NULL,'62288e1a6073d2266199abe35ff9e8c47fb3c5bcddf1cb456ca2e6dabe407ed9',1),(21,'Стейк Рибай',NULL,'Мраморная говядина, прожарка на выбор',1650.00,7,1,'440057367eff955b43e39dcbda55f9b61ad48277ed69380e79c97bbfb65ad890',1),(22,'Шницель Венский',NULL,'Традиционный шницель из телятины',890.00,7,NULL,'2a41620872a244cc2b2e96a3ac34e98bf37d12d7a247bdabdbed4a5ee9fb2f54',1),(23,'Котлета по-киевски',NULL,'Курица с маслом и зеленью внутри',540.00,7,2,'312c6979a9eb57f092dddc79633eb1bcd9ca3558e722dec8762b3f449ce474f6',1),(24,'Утка Конфи',NULL,'Французское блюдо из утки, томлённой в собственном жире',1450.00,7,NULL,'e5198bbe8ccc5073749850d3cefa57ed68e1f04d1a89580ceacbfd1467ee388a',1),(25,'Форель на гриле',NULL,'Форель с лимоном и травами',890.00,8,1,'265a1e40e44427ec36e127b50ed88899c16f25c92fa062dd0f707245fbf49ef8',1),(26,'Филе трески в сливочном соусе',NULL,'Подаётся с овощами',760.00,8,NULL,'e8910fd1a762ad0a13844dd9336fce1166eb8ab85329e29c8b0d0fbb3d4c9094',1),(27,'Палтус под соусом терияки',NULL,'Сезонное предложение',1100.00,8,1,'5d43e60ca93f4607ceea7c54da1adc76c501d2b7be9523d959368fc1f5812672',1),(28,'Кальмары гриль',NULL,'Кальмары с чесночным соусом',680.00,8,NULL,'5be119d29caf0f43fdaa172eafc6c048afc932411ce32c520e897fbbc07255ad',1),(29,'Картофель по-деревенски',NULL,'Запечённый картофель с чесноком и укропом',250.00,9,NULL,'0998064fb9332a0ad88aefa4490cfb796049e208af1c63d43b1ddc1803add299',1),(30,'Овощи гриль',NULL,'Кабачки, баклажаны, перец, томаты',320.00,9,4,'86638f9b738f05f3240352775b0d4f2125784292de3fc536b132672a705faf84',1),(31,'Ризотто с грибами',NULL,'Итальянский рис с белыми грибами',520.00,9,NULL,'e870e2d5be358de9bc7da43f7f62583b6568fd298971426f9b7ee3f624942905',1),(32,'Тирамису',NULL,'Классический итальянский десерт',390.00,10,2,'172d5ffe08b0388fe3f07235669a7636e69b8fd9f97288d7491cc005dc6e918d',1),(33,'Чизкейк Нью-Йорк',NULL,'Сливочный чизкейк с клубничным соусом',420.00,10,NULL,'93e54643a70c2433be72b98302f3be5b5483344e3046223e6da8141642caaa50',1),(34,'Панна Котта',NULL,'Итальянский сливочный десерт',370.00,10,NULL,'0ea0014dcc855e8b2393ad8ae26a534c13e131a6dca2f773cc3bb635fc59c640',1),(35,'Крем-брюле',NULL,'Французский десерт с карамельной корочкой',450.00,10,1,'5a5886d3785300535307e28eed2d37f2ab407897640ca402c95d11f3f7358dc6',1),(36,'Штрудель яблочный',NULL,'С яблоками и корицей, подаётся с мороженым',410.00,10,NULL,'0285acd1399952095fb8d0f69049164cda2d102b789c2a73fc03a8c2cd538786',1),(37,'Соус Песто',NULL,'Традиционный соус из базилика и орехов',120.00,11,NULL,'da9f5a9843d6ae7adf62837eec18bc3ec385900b86c92bfe8cdac1d391f6410b',1),(38,'Соус Сальса',NULL,'Острый томатный соус',100.00,11,NULL,'14948935470a1643ba3f62a4d80af046a42d496770b534bdeea53fa69219e287',1),(39,'Соус Тартар',NULL,'Майонез, корнишоны, зелень',90.00,11,NULL,'23604e3461de77ceab6d89f81200f146c040b6190bd7bee09a9d3cbdf9f23d2d',1),(40,'Чиабатта',NULL,'Итальянский пшеничный хлеб',80.00,12,NULL,'36296690abfb271dd0cdafb00ab6b5878ab8ea2e3ad3ce5b8742cdd2784e1544',1),(41,'Фокачча с розмарином',NULL,'Тонкая лепёшка с оливковым маслом',110.00,12,4,'4ffadbddd6189a34473b52bbdb43a53a0ad901e18d1a7b78655618bc34251113',1),(42,'Булочки с чесноком',NULL,'Свежая выпечка к супам и закускам',90.00,12,NULL,'120e55380756de3a5a8c7525553d90cdfd3f56ea08366dae6ad1a077a4486236',1),(43,'Ассорти из итальянских сыров',NULL,'Пармезан, горгонзола, моцарелла',560.00,13,3,'b9384d2a332f30f2140492f4b27a8eefc782d0e2aa0445efc41880b6b7452b14',1),(44,'Ассорти из французских сыров',NULL,'Камамбер, бри, рокфор',620.00,13,NULL,'43e38fe1686d740dfcd082d750b8c6c01818c86300d687cc98cd9420f44f606c',1),(45,'Эспрессо',NULL,'Кофе крепкий',150.00,14,NULL,'7579bd62b17372e25ba02d80e1c2ae98af947a31e9d6848d6eab0f50fed29677',1),(46,'Капучино',NULL,'Кофе с молочной пеной',190.00,14,NULL,'dd122e917e864ae902ddd8704e1cba3286df66321e61d7f0d90376b7ca7f1d9f',1),(47,'Апельсиновый фреш',NULL,'Свежевыжатый сок',250.00,14,1,'193b56779b23659ce537d72159d13e82404970b8d357347f54a007799fe136f7',1),(48,'Минеральная вода',NULL,'Газированная/негазированная',120.00,14,NULL,'181b45ff324355725e06ca4eac33da2dfb57a44b64f19755588fa971a72af26a',1),(49,'Шардоне',NULL,'Белое сухое вино, Франция',950.00,15,NULL,'bd165823730747c3363f924ae3cb137cd4a470a6299adc95c45cc06957a8d612',1),(50,'Кьянти',NULL,'Красное сухое вино, Италия',1100.00,15,5,'60578ce864e30e3f05873677aaaed1f3647128647fff667185c53d7ccaf3c59a',1),(51,'Просекко',NULL,'Игристое вино, Италия',1250.00,15,5,'48469143271ccc9d3a793407382d9299c6c765a2d48762b3ec86ce01cdf422cb',1),(52,'Сангрия',NULL,'Испанский напиток с фруктами и вином',890.00,15,3,'d3791644791296ed85db6a6ce55fcd2e0dbfd4ed87e6f86a22f67355055161dc',1);
/*!40000 ALTER TABLE `menudish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `offersdish`
--

DROP TABLE IF EXISTS `offersdish`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `offersdish` (
  `OffersDishId` int NOT NULL AUTO_INCREMENT,
  `OffersDishName` varchar(50) NOT NULL,
  `OffersDishDicsount` int NOT NULL,
  PRIMARY KEY (`OffersDishId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `offersdish`
--

LOCK TABLES `offersdish` WRITE;
/*!40000 ALTER TABLE `offersdish` DISABLE KEYS */;
INSERT INTO `offersdish` VALUES (1,'Сезонное',5),(2,'Комбо',5),(3,'Праздничные акции',10),(4,'Диетическое',15),(5,'Тематический набор',10);
/*!40000 ALTER TABLE `offersdish` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order` (
  `OrderId` int NOT NULL AUTO_INCREMENT,
  `WorkerId` int NOT NULL,
  `ClientId` int DEFAULT NULL,
  `TableId` int NOT NULL,
  `OrderDate` datetime NOT NULL,
  `OrderPrice` decimal(10,2) NOT NULL,
  `OrderStatus` enum('Новый','Завершен','Отменен') NOT NULL,
  `OrderStatusPayment` enum('Оплачен','Не оплачен') NOT NULL,
  PRIMARY KEY (`OrderId`),
  KEY `order_ibfk_1` (`WorkerId`),
  KEY `order_ibfk_2` (`ClientId`),
  KEY `order_ibfk_3` (`TableId`),
  CONSTRAINT `order_ibfk_1` FOREIGN KEY (`WorkerId`) REFERENCES `worker` (`WorkerId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `order_ibfk_2` FOREIGN KEY (`ClientId`) REFERENCES `client` (`ClientId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `order_ibfk_3` FOREIGN KEY (`TableId`) REFERENCES `tables` (`TablesId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (1,32,1,1,'2026-04-28 18:45:00',3508.50,'Завершен','Оплачен'),(2,34,NULL,2,'2026-04-28 19:10:00',1690.00,'Новый','Не оплачен'),(3,30,NULL,3,'2026-04-27 13:30:00',4552.50,'Отменен','Не оплачен'),(4,39,NULL,4,'2026-04-27 20:15:00',2270.00,'Завершен','Оплачен'),(5,21,5,5,'2026-04-26 17:20:00',1885.00,'Новый','Не оплачен'),(6,23,NULL,6,'2024-11-18 12:40:00',1307.50,'Завершен','Оплачен'),(7,40,NULL,7,'2026-04-26 21:05:00',2535.50,'Отменен','Не оплачен'),(8,29,NULL,8,'2025-09-25 19:55:00',1452.50,'Завершен','Оплачен'),(9,34,9,9,'2026-04-25 18:25:00',2561.00,'Новый','Не оплачен'),(10,36,10,10,'2026-04-25 20:45:00',1850.00,'Завершен','Оплачен'),(11,29,11,11,'2025-07-12 13:15:00',1948.00,'Отменен','Не оплачен'),(12,22,12,12,'2026-04-24 14:10:00',1541.50,'Новый','Не оплачен'),(13,33,13,13,'2026-04-24 15:30:00',1876.00,'Завершен','Оплачен'),(14,40,14,14,'2025-12-13 18:00:00',2744.00,'Завершен','Оплачен'),(15,24,15,15,'2026-04-23 20:35:00',1875.00,'Новый','Не оплачен'),(16,24,16,16,'2025-04-16 19:25:00',1700.00,'Отменен','Не оплачен'),(17,35,17,17,'2026-04-23 18:50:00',2764.00,'Завершен','Оплачен'),(18,35,18,18,'2026-04-22 21:15:00',1722.00,'Новый','Не оплачен'),(19,32,19,19,'2025-02-05 17:10:00',1830.00,'Завершен','Оплачен'),(20,40,20,20,'2026-04-22 13:35:00',2307.50,'Отменен','Не оплачен'),(21,21,21,1,'2026-04-21 20:05:00',2099.50,'Новый','Не оплачен'),(22,35,22,2,'2024-12-15 19:40:00',3351.50,'Завершен','Оплачен'),(23,34,23,3,'2026-04-21 18:20:00',3212.50,'Новый','Не оплачен'),(24,27,24,4,'2026-04-20 21:03:41',1690.50,'Завершен','Оплачен'),(25,29,25,5,'2025-09-24 21:03:41',1230.00,'Отменен','Не оплачен'),(26,37,26,6,'2026-04-20 19:30:00',1465.50,'Новый','Не оплачен'),(27,24,27,7,'2024-10-14 21:03:41',2815.00,'Завершен','Оплачен'),(28,30,28,8,'2026-04-19 16:45:00',1850.00,'Завершен','Оплачен'),(29,34,29,9,'2025-08-14 21:03:41',2193.00,'Отменен','Не оплачен'),(30,37,30,10,'2026-04-19 14:20:00',3375.00,'Новый','Не оплачен'),(31,28,31,11,'2024-08-19 21:03:41',4065.00,'Завершен','Оплачен'),(32,27,32,12,'2026-04-18 13:10:00',2413.50,'Новый','Не оплачен'),(33,24,33,13,'2026-04-18 17:55:00',1920.00,'Завершен','Оплачен'),(34,26,34,14,'2025-07-03 21:03:41',1759.00,'Отменен','Не оплачен'),(35,24,35,15,'2026-04-17 12:25:00',2350.50,'Новый','Не оплачен'),(36,38,36,16,'2024-05-14 21:03:41',2331.00,'Завершен','Оплачен'),(37,34,37,17,'2026-04-17 19:40:00',2071.00,'Новый','Не оплачен'),(38,23,38,18,'2025-07-04 21:03:41',1762.50,'Отменен','Не оплачен'),(39,34,39,19,'2026-04-16 14:15:00',2460.50,'Завершен','Оплачен'),(40,39,40,20,'2026-04-16 20:30:00',2153.50,'Новый','Не оплачен'),(41,32,41,1,'2026-04-15 18:45:00',2171.00,'Завершен','Оплачен'),(42,27,42,2,'2025-03-23 21:03:41',1140.00,'Отменен','Не оплачен'),(43,31,43,3,'2026-04-15 13:20:00',2075.00,'Новый','Не оплачен'),(44,32,44,4,'2024-10-23 21:03:41',2066.00,'Завершен','Оплачен'),(45,32,NULL,5,'2026-04-14 19:00:00',2203.00,'Завершен','Оплачен'),(46,33,46,6,'2025-06-08 21:03:41',2477.50,'Новый','Не оплачен'),(47,38,NULL,7,'2026-04-14 12:50:00',1662.00,'Отменен','Не оплачен'),(48,30,NULL,8,'2026-04-13 21:10:00',3285.00,'Завершен','Оплачен'),(49,30,49,9,'2025-04-02 21:03:41',2655.00,'Новый','Не оплачен'),(50,29,NULL,10,'2026-04-13 17:35:00',1609.50,'Завершен','Оплачен');
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orderitems`
--

DROP TABLE IF EXISTS `orderitems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orderitems` (
  `OrderId` int NOT NULL,
  `DishId` int NOT NULL,
  `DishCount` int NOT NULL,
  `OriginalPrice` decimal(10,2) NOT NULL DEFAULT '0.00',
  `OriginalDiscount` int NOT NULL DEFAULT '0',
  `OriginalDishName` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`OrderId`,`DishId`),
  KEY `orderitems_ibfk_2` (`DishId`),
  CONSTRAINT `orderitems_ibfk_1` FOREIGN KEY (`OrderId`) REFERENCES `order` (`OrderId`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `orderitems_ibfk_2` FOREIGN KEY (`DishId`) REFERENCES `menudish` (`DishId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orderitems`
--

LOCK TABLES `orderitems` WRITE;
/*!40000 ALTER TABLE `orderitems` DISABLE KEYS */;
INSERT INTO `orderitems` VALUES (1,1,2,450.00,0,'Цезарь с курицей'),(1,21,1,1650.00,5,'Стейк Рибай'),(1,32,2,390.00,5,'Тирамису'),(1,45,2,150.00,0,'Эспрессо'),(2,3,1,520.00,0,'Салат Нисуаз'),(2,17,2,490.00,5,'Пицца Маргарита'),(2,46,1,190.00,0,'Капучино'),(3,21,2,1650.00,5,'Стейк Рибай'),(3,35,1,450.00,5,'Крем-брюле'),(3,50,1,1100.00,10,'Кьянти'),(4,7,2,360.00,0,'Жульен с грибами'),(4,22,1,890.00,0,'Шницель Венский'),(4,33,1,420.00,0,'Чизкейк Нью-Йорк'),(4,48,2,120.00,0,'Минеральная вода'),(5,11,1,860.00,15,'Тар-тар из лосося'),(5,18,2,560.00,5,'Пицца Пепперони'),(5,39,1,90.00,0,'Соус Тартар'),(6,2,2,390.00,15,'Греческий салат'),(6,15,1,580.00,5,'Лазанья Болоньезе'),(6,41,1,110.00,15,'Фокачча с розмарином'),(7,8,1,890.00,5,'Креветки в чесночном соусе'),(7,24,1,1450.00,0,'Утка Конфи'),(7,37,2,120.00,0,'Соус Песто'),(8,4,1,350.00,5,'Минестроне'),(8,29,2,250.00,0,'Картофель по-деревенски'),(8,44,1,620.00,0,'Ассорти из французских сыров'),(9,13,2,490.00,5,'Спагетти Карбонара'),(9,28,1,680.00,0,'Кальмары гриль'),(9,49,1,950.00,0,'Шардоне'),(10,5,1,420.00,0,'Французский луковый суп'),(10,20,2,670.00,0,'Пицца Капричоза'),(10,42,1,90.00,0,'Булочки с чесноком'),(11,9,1,950.00,5,'Мидии по-провански'),(11,25,1,890.00,5,'Форель на гриле'),(11,38,2,100.00,0,'Соус Сальса'),(12,6,2,380.00,0,'Борщ европейский'),(12,19,1,640.00,15,'Пицца Четыре сыра'),(12,47,1,250.00,5,'Апельсиновый фреш'),(13,14,1,440.00,0,'Пенне Арабьята'),(13,23,2,540.00,5,'Котлета по-киевски'),(13,36,1,410.00,0,'Штрудель яблочный'),(14,10,1,720.00,0,'Карпаччо из говядины'),(14,26,2,760.00,0,'Филе трески в сливочном соусе'),(14,43,1,560.00,10,'Ассорти из итальянских сыров'),(15,12,1,670.00,0,'Антипасти ассорти'),(15,27,1,1100.00,5,'Палтус под соусом терияки'),(15,40,2,80.00,0,'Чиабатта'),(16,1,1,450.00,0,'Цезарь с курицей'),(16,16,1,510.00,0,'Фетучини Альфредо'),(16,34,2,370.00,0,'Панна Котта'),(17,8,2,890.00,5,'Креветки в чесночном соусе'),(17,30,1,320.00,15,'Овощи гриль'),(17,52,1,890.00,10,'Сангрия'),(18,3,2,520.00,0,'Салат Нисуаз'),(18,18,1,560.00,5,'Пицца Пепперони'),(18,45,1,150.00,0,'Эспрессо'),(19,5,2,420.00,0,'Французский луковый суп'),(19,22,1,890.00,0,'Шницель Венский'),(19,38,1,100.00,0,'Соус Сальса'),(20,7,1,360.00,0,'Жульен с грибами'),(20,21,1,1650.00,5,'Стейк Рибай'),(20,46,2,190.00,0,'Капучино'),(21,11,2,860.00,15,'Тар-тар из лосося'),(21,19,1,640.00,15,'Пицца Четыре сыра'),(21,41,1,110.00,15,'Фокачча с розмарином'),(22,2,1,390.00,15,'Греческий салат'),(22,24,2,1450.00,0,'Утка Конфи'),(22,37,1,120.00,0,'Соус Песто'),(23,9,1,950.00,5,'Мидии по-провански'),(23,28,2,680.00,0,'Кальмары гриль'),(23,49,1,950.00,0,'Шардоне'),(24,4,2,350.00,5,'Минестроне'),(24,25,1,890.00,5,'Форель на гриле'),(24,42,2,90.00,0,'Булочки с чесноком'),(25,6,1,380.00,0,'Борщ европейский'),(25,20,1,670.00,0,'Пицца Капричоза'),(25,39,2,90.00,0,'Соус Тартар'),(26,13,1,490.00,5,'Спагетти Карбонара'),(26,26,1,760.00,0,'Филе трески в сливочном соусе'),(26,48,2,120.00,0,'Минеральная вода'),(27,10,2,720.00,0,'Карпаччо из говядины'),(27,29,1,250.00,0,'Картофель по-деревенски'),(27,51,1,1250.00,10,'Просекко'),(28,1,1,450.00,0,'Цезарь с курицей'),(28,17,2,490.00,5,'Пицца Маргарита'),(28,33,1,420.00,0,'Чизкейк Нью-Йорк'),(29,14,1,440.00,0,'Пенне Арабьята'),(29,23,1,540.00,5,'Котлета по-киевски'),(29,44,2,620.00,0,'Ассорти из французских сыров'),(30,12,2,670.00,0,'Антипасти ассорти'),(30,27,1,1100.00,5,'Палтус под соусом терияки'),(30,50,1,1100.00,10,'Кьянти'),(31,3,1,520.00,0,'Салат Нисуаз'),(31,21,2,1650.00,5,'Стейк Рибай'),(31,36,1,410.00,0,'Штрудель яблочный'),(32,8,1,890.00,5,'Креветки в чесночном соусе'),(32,18,2,560.00,5,'Пицца Пепперони'),(32,43,1,560.00,10,'Ассорти из итальянских сыров'),(33,5,2,420.00,0,'Французский луковый суп'),(33,22,1,890.00,0,'Шницель Венский'),(33,46,1,190.00,0,'Капучино'),(34,7,1,360.00,0,'Жульен с грибами'),(34,19,1,640.00,15,'Пицца Четыре сыра'),(34,35,2,450.00,5,'Крем-брюле'),(35,2,2,390.00,15,'Греческий салат'),(35,24,1,1450.00,0,'Утка Конфи'),(35,47,1,250.00,5,'Апельсиновый фреш'),(36,11,1,860.00,15,'Тар-тар из лосося'),(36,26,2,760.00,0,'Филе трески в сливочном соусе'),(36,40,1,80.00,0,'Чиабатта'),(37,6,2,380.00,0,'Борщ европейский'),(37,16,1,510.00,0,'Фетучини Альфредо'),(37,52,1,890.00,10,'Сангрия'),(38,9,1,950.00,5,'Мидии по-провански'),(38,28,1,680.00,0,'Кальмары гриль'),(38,39,2,90.00,0,'Соус Тартар'),(39,4,2,350.00,5,'Минестроне'),(39,25,1,890.00,5,'Форель на гриле'),(39,49,1,950.00,0,'Шардоне'),(40,10,1,720.00,0,'Карпаччо из говядины'),(40,20,2,670.00,0,'Пицца Капричоза'),(40,41,1,110.00,15,'Фокачча с розмарином'),(41,13,2,490.00,5,'Спагетти Карбонара'),(41,29,1,250.00,0,'Картофель по-деревенски'),(41,50,1,1100.00,10,'Кьянти'),(42,1,1,450.00,0,'Цезарь с курицей'),(42,17,1,490.00,5,'Пицца Маргарита'),(42,38,2,100.00,0,'Соус Сальса'),(43,14,2,440.00,0,'Пенне Арабьята'),(43,27,1,1100.00,5,'Палтус под соусом терияки'),(43,45,1,150.00,0,'Эспрессо'),(44,5,1,420.00,0,'Французский луковый суп'),(44,23,2,540.00,5,'Котлета по-киевски'),(44,44,1,620.00,0,'Ассорти из французских сыров'),(45,8,2,890.00,5,'Креветки в чесночном соусе'),(45,30,1,320.00,15,'Овощи гриль'),(45,48,2,120.00,0,'Минеральная вода'),(46,12,1,670.00,0,'Антипасти ассорти'),(46,21,1,1650.00,5,'Стейк Рибай'),(46,37,2,120.00,0,'Соус Песто'),(47,3,2,520.00,0,'Салат Нисуаз'),(47,18,1,560.00,5,'Пицца Пепперони'),(47,42,1,90.00,0,'Булочки с чесноком'),(48,6,1,380.00,0,'Борщ европейский'),(48,22,2,890.00,0,'Шницель Венский'),(48,51,1,1250.00,10,'Просекко'),(49,9,2,950.00,5,'Мидии по-провански'),(49,26,1,760.00,0,'Филе трески в сливочном соусе'),(49,39,1,90.00,0,'Соус Тартар'),(50,2,1,390.00,15,'Греческий салат'),(50,19,2,640.00,15,'Пицца Четыре сыра'),(50,46,1,190.00,0,'Капучино');
/*!40000 ALTER TABLE `orderitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `RoleId` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(20) NOT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Администратор'),(2,'Менеджер'),(3,'Официант'),(4,'Шеф-повар');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tables`
--

DROP TABLE IF EXISTS `tables`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tables` (
  `TablesId` int NOT NULL AUTO_INCREMENT,
  `TablesCountPlace` int NOT NULL,
  `TablesStatus` enum('Свободен','Забронирован','Занят') NOT NULL,
  PRIMARY KEY (`TablesId`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tables`
--

LOCK TABLES `tables` WRITE;
/*!40000 ALTER TABLE `tables` DISABLE KEYS */;
INSERT INTO `tables` VALUES (1,2,'Занят'),(2,4,'Занят'),(3,6,'Занят'),(4,4,'Занят'),(5,8,'Занят'),(6,4,'Занят'),(7,2,'Занят'),(8,6,'Свободен'),(9,4,'Занят'),(10,8,'Занят'),(11,2,'Занят'),(12,4,'Занят'),(13,6,'Свободен'),(14,8,'Занят'),(15,2,'Занят'),(16,4,'Занят'),(17,6,'Занят'),(18,2,'Занят'),(19,8,'Свободен'),(20,4,'Занят');
/*!40000 ALTER TABLE `tables` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `worker`
--

DROP TABLE IF EXISTS `worker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `worker` (
  `WorkerId` int NOT NULL AUTO_INCREMENT,
  `WorkerFIO` varchar(100) NOT NULL,
  `OriginalWorkerFIO` varchar(100) DEFAULT NULL,
  `WorkerLogin` varchar(50) NOT NULL,
  `WorkerPassword` varchar(200) NOT NULL,
  `WorkerPhone` varchar(11) NOT NULL,
  `WorkerPassport` varchar(11) NOT NULL COMMENT 'Паспортные данные',
  `WorkerRole` int NOT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`WorkerId`),
  KEY `worker_ibfk_1` (`WorkerRole`),
  CONSTRAINT `worker_ibfk_1` FOREIGN KEY (`WorkerRole`) REFERENCES `role` (`RoleId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `worker`
--

LOCK TABLES `worker` WRITE;
/*!40000 ALTER TABLE `worker` DISABLE KEYS */;
INSERT INTO `worker` VALUES (1,'Бартенев Антон Ильич',NULL,'admin1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000001','4510 123456',1,1),(2,'Фролова Мария Александровна',NULL,'admin2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000002','4510 234567',1,1),(3,'Кузнецов Олег Игоревич',NULL,'admin3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000003','4511 345678',1,1),(4,'Морозова Екатерина Владимировна',NULL,'admin4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000004','4511 456789',1,1),(5,'Волков Антон Сергеевич',NULL,'admin5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000005','4512 567890',1,1),(6,'Иванов Павел Константинович',NULL,'manager1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000006','4512 678901',2,1),(7,'Егорова Светлана Михайловна',NULL,'manager2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000007','4513 789012',2,1),(8,'Попов Дмитрий Олегович',NULL,'manager3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000008','4513 890123',2,1),(9,'Соколова Ирина Фёдоровна',NULL,'manager4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000009','4514 901234',2,1),(10,'Зайцев Владислав Романович',NULL,'manager5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000010','4514 012345',2,1),(11,'Орлова Анастасия Васильевна',NULL,'manager6','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000011','4515 123456',2,1),(12,'Лебедев Сергей Андреевич',NULL,'manager7','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000012','4515 234567',2,1),(13,'Крылова Ольга Дмитриевна',NULL,'manager8','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000013','4516 345678',2,1),(14,'Фомин Никита Евгеньевич',NULL,'manager9','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000014','4516 456789',2,1),(15,'Васильева Татьяна Ивановна',NULL,'manager10','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000015','4517 567890',2,1),(16,'Смирнов Алексей Петрович','Смирнов Алексей Петрович','manager11','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000016','4517 678901',2,1),(17,'Новикова Юлия Александровна',NULL,'manager12','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000017','4518 789012',2,1),(18,'Сергеев Григорий Дмитриевич',NULL,'manager13','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000018','4518 890123',2,1),(19,'Белова Ксения Фёдоровна',NULL,'manager14','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000019','4519 901234',2,1),(20,'Козлов Иван Олегович',NULL,'manager15','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000020','4519 012345',2,1),(21,'Медведева Алина Романовна',NULL,'waiter1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000021','4520 123456',3,1),(22,'Борисов Николай Александрович',NULL,'waiter2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000022','4520 234567',3,1),(23,'Григорьева Вера Игоревна',NULL,'waiter3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000023','4521 345678',3,1),(24,'Алексеев Роман Михайлович',NULL,'waiter4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000024','4521 456789',3,1),(25,'Ефимова Полина Андреевна',NULL,'waiter5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000025','4522 567890',3,1),(26,'Никитин Артём Ильич',NULL,'waiter6','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000026','4522 678901',3,1),(27,'Кузьмина Елена Павловна',NULL,'waiter7','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000027','4523 789012',3,1),(28,'Макаров Фёдор Игоревич',NULL,'waiter8','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000028','4523 890123',3,1),(29,'Дмитриева Карина Алексеевна',NULL,'waiter9','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000029','4524 901234',3,1),(30,'Поляков Степан Олегович',NULL,'waiter10','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000030','4524 012345',3,1),(31,'Захарова Марина Ильинична',NULL,'waiter11','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000031','4525 123456',3,1),(32,'Комаров Даниил Фёдорович',NULL,'waiter12','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000032','4525 234567',3,1),(33,'Богданова Людмила Сергеевна',NULL,'waiter13','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000033','4526 345678',3,1),(34,'Савельев Игорь Викторович',NULL,'waiter14','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000034','4526 456789',3,1),(35,'Романова Дарья Анатольевна',NULL,'waiter15','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000035','4527 567890',3,1),(36,'Миронов Валерий Андреевич',NULL,'waiter16','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000036','4527 678901',3,1),(37,'Тихонова Алёна Ивановна',NULL,'waiter17','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000037','4528 789012',3,1),(38,'Гаврилов Виталий Степанович',NULL,'waiter18','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000038','4528 890123',3,1),(39,'Федорова Ангелина Дмитриевна',NULL,'waiter19','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000039','4529 901234',3,1),(40,'Соболев Михаил Николаевич',NULL,'waiter20','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000040','4529 012345',3,1),(41,'Трифонова Инна Андреевна',NULL,'chef1','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000041','4530 123456',4,1),(42,'Мельников Георгий Васильевич',NULL,'chef2','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000042','4530 234567',4,1),(43,'Ковалева Лилия Евгеньевна',NULL,'chef3','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000043','4531 345678',4,1),(44,'Васильев Станислав Тимурович',NULL,'chef4','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000044','4531 456789',4,1),(45,'Голубева Виктория Аркадьевна',NULL,'chef5','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000045','4532 567890',4,1),(46,'Власов Арсений Григорьевич',NULL,'chef6','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000046','4532 678901',4,1),(47,'Сидорова Инна Никитична',NULL,'chef7','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000047','4533 789012',4,1),(48,'Павлов Вадим Романович',NULL,'chef8','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000048','4533 890123',4,1),(49,'Цветкова Евгения Владимировна',NULL,'chef9','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000049','4534 901234',4,1),(50,'Баринов Виктор Петрович',NULL,'chef10','7549920a8f8c5dec3f1dcdb7a5eb7840ea1e52f2ee40fe70b6d1fb376aed3a8d','79010000050','4534 012345',4,1);
/*!40000 ALTER TABLE `worker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'db57'
--

--
-- Dumping routines for database 'db57'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-05-12 20:05:05
