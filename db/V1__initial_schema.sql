CREATE DATABASE IF NOT EXISTS cine;
USE cine;

DROP PROCEDURE IF EXISTS spInsSucursales;
DROP VIEW IF EXISTS vwRptSucursales;
DROP TABLE IF EXISTS SUCURSAL;

CREATE TABLE SUCURSAL
(
    SUC_CVE_SUCURSAL INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    SUC_NOMBRE VARCHAR(50) NOT NULL,
    SUC_DIRECCION VARCHAR(100) NOT NULL,
    SUC_HOMEWEB VARCHAR(100) NOT NULL,
    SUC_LOGO VARCHAR(100) NOT NULL
);

-- 1. Insertar 10 sucursales
INSERT INTO SUCURSAL
(SUC_NOMBRE, SUC_DIRECCION, SUC_HOMEWEB, SUC_LOGO)
VALUES
('Cinepolis Galerias Pachuca',
 'Blvd. Colosio, Pachuca, Hidalgo',
 'https://www.cinepolis.com/',
 'imagenes/logo1.jpg'),

('Cinemex Pachuca',
 'Zona Plateada, Pachuca, Hidalgo',
 'https://cinemex.com/',
 'imagenes/logo2.jpg'),

('Cinepolis Plaza Universidad',
 'Pachuca de Soto, Hidalgo',
 'https://cinepolis.com/',
 'imagenes/logo3.jpg'),

('Cinepolis Gran Patio',
 'Carretera Mexico-Pachuca, Hidalgo',
 'https://www.cinepolis.com/formatos/',
 'imagenes/logo4.jpg'),

('Cinemex Explanada',
 'San Antonio el Desmonte, Hidalgo',
 'https://www.cinemex.com/',
 'imagenes/logo5.jpg'),

('Cinepolis Plaza Bella',
 'Pachuca de Soto, Hidalgo',
 'https://cinepolis.com/experiencia/',
 'imagenes/logo6.jpg'),

('Cinepolis Tulancingo',
 'Tulancingo de Bravo, Hidalgo',
 'https://www.cinepolis.com/cinepolis-vip/',
 'imagenes/logo7.jpg'),

('Cinemex Tizayuca',
 'Tizayuca, Hidalgo',
 'https://cinemex.com/peliculas/',
 'imagenes/logo8.jpg'),

('Cinepolis Actopan',
 'Actopan, Hidalgo',
 'https://cinepolis.com/cartelera/',
 'imagenes/logo9.jpg'),

('Cinepolis Mineral de la Reforma',
 'Mineral de la Reforma, Hidalgo',
 'https://www.cinepolis.com/',
 'imagenes/logo10.jpg');

-- 2. Crear la vista ordenada por nombre
CREATE VIEW vwRptSucursales AS
SELECT *
FROM SUCURSAL
ORDER BY SUC_NOMBRE;

-- 3. Crear el procedimiento almacenado
DELIMITER $$

CREATE PROCEDURE spInsSucursales
(
    IN p_nombre VARCHAR(50),
    IN p_direccion VARCHAR(100),
    IN p_homeweb VARCHAR(100),
    IN p_logo VARCHAR(100),
    OUT p_bandera INT
)
BEGIN
    -- Bandera 1: nombre duplicado
    IF EXISTS (
        SELECT 1
        FROM SUCURSAL
        WHERE SUC_NOMBRE = p_nombre
    ) THEN
        SET p_bandera = 1;

    -- Bandera 2: URL duplicada
    ELSEIF EXISTS (
        SELECT 1
        FROM SUCURSAL
        WHERE SUC_HOMEWEB = p_homeweb
    ) THEN
        SET p_bandera = 2;

    -- Bandera 0: registro insertado correctamente
    ELSE
        INSERT INTO SUCURSAL
        (SUC_NOMBRE, SUC_DIRECCION, SUC_HOMEWEB, SUC_LOGO)
        VALUES
        (p_nombre, p_direccion, p_homeweb, p_logo);

        SET p_bandera = 0;
    END IF;
END $$

DELIMITER ;

-- 4. Consultar los registros
SELECT * FROM SUCURSAL;

-- 5. Consultar la vista
SELECT * FROM vwRptSucursales;

-- 6. Probar el procedimiento
CALL spInsSucursales(
    'Cine de prueba',
    'Pachuca, Hidalgo',
    'https://ejemplo.com/cine-prueba',
    'imagenes/prueba.jpg',
    @bandera
);

SELECT @bandera AS resultado;
