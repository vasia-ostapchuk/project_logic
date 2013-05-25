select Mark AS Марка,Color AS Колір,License_plate AS Номер, Type_kpp AS тип_КПП, Motor AS Двигун, Vypusk AS Рік_випуску, Places AS Мість, Litr_on_100 AS витрати_на_100, Cars.Price AS Ціна_прокату FROM Cars where 
                         Car_ID NOT IN (select Car_IDFK FROM Car_reservation where  Status ='дійсна' 
                         AND((Date_end >= CAST('"+value.date_beginning+"' AS date) and Date_end <= CAST('"+value.date_end+"' AS date)) 
                         OR (Date_bginning >= CAST('"+value.date_beginning+"' AS date) and Date_bginning <= CAST('"+value.date_end+"' AS date)))) 
                         AND Rental_Point_IDFK IN (select Rental_Point_ID from Rental_Point where LocationFK Like '" + value.nameLocation + "%' AND Rental_NameFK LIKE '"+Rental_name+"%')
