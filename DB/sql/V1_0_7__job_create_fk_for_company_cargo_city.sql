ALTER TABLE `scs_logbook`.`city` 
ADD CONSTRAINT `name_UNIQUE` UNIQUE (`name`);
;

ALTER TABLE `scs_logbook`.`cargo` 
ADD CONSTRAINT `name_UNIQUE` UNIQUE (`name`);
;

ALTER TABLE `scs_logbook`.`company` 
ADD CONSTRAINT `name_UNIQUE` UNIQUE (`name`);
;


ALTER TABLE `scs_logbook`.`job` 
ADD CONSTRAINT `fk_city_source`
  FOREIGN KEY (`citysourceid`)
  REFERENCES `scs_logbook`.`city` (`idcity`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_city_destination`
  FOREIGN KEY (`citydestinationid`)
  REFERENCES `scs_logbook`.`city` (`idcity`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_cargo`
  FOREIGN KEY (`cargoid`)
  REFERENCES `scs_logbook`.`cargo` (`idcargo`)
  ON DELETE RESTRICT
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_company_destination`
  FOREIGN KEY (`companydestinationid`)
  REFERENCES `scs_logbook`.`company` (`idcompany` )
  ON DELETE RESTRICT
  ON UPDATE NO ACTION,
ADD CONSTRAINT `fk_company_source`
  FOREIGN KEY ( `companysourceid`)
  REFERENCES `scs_logbook`.`company` (`idcompany`)
  ON DELETE RESTRICT
  ON UPDATE NO ACTION;