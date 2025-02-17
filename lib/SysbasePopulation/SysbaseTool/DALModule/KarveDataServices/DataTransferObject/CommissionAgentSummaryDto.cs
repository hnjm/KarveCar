﻿using System.ComponentModel.DataAnnotations;

namespace KarveDataServices.DataTransferObject
{

    /// <summary>
    ///  CommissinAgent Summary Data Transfer object.
    /// </summary>
    public class CommissionAgentSummaryDto: BaseDto
    {

        /*
         SELECT NUM_COMI as Number, NOMBRE as Name, PERSONA as Persona, NIF as Nif, DIRECCION as Direction, PROVINCIA.CP as Zip, POBLACION as City, PROVINCIA.PROV as Province, PAIS.PAIS as Country,IATA, SUBLICEN as Company,  ZONAOFI as OfficeZone, COMISIO.ULTMODI as LastModification, COMISIO.USUARIO as CurrentUser  FROM COMISIO LEFT OUTER JOIN PROVINCIA ON COMISIO.PROVINCIA = PROVINCIA.SIGLAS LEFT OUTER JOIN PAIS on COMISIO.NACIOPER = PAIS.SIGLAS;   
             */
        [Display(Name = "Numero Commissionista")]
        public string Code { set; get; }
        [Display(Name = "Nombre Commisionista")]
        public string Name { set; get; }
        [Display(Name = "Persona riferimento")]
        public string Person { set; get; }
        [Display(Name = "Nif")]
        public string Nif { set; get; }
        [Display(Name = "Direccion")]
        public string Direction { set; get; }
        [Display(Name = "CP")]
        public string Zip { set; get; }
        [Display(Name = "Poblacion")]
        public string City { set; get; }
        [Display(Name = "Provincia")]
        public string Province { set; get; }
        [Display(Name = "Pais")]
        public string Country { set; get; }
        [Display(Name = "N.IATA")]
        public string IATA { set; get; }
        [Display(Name = "Empresa")]
        public string Company { set; get; }
        [Display(Name = "Zona")]
        public string OfficeZone { set; get; }
    }
}
