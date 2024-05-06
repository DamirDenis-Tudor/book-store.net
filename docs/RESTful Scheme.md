PS: Input si Output sunt JSON
##### POST api/login: 
- Input: user, password (as hash)
- Output:
	- 200 OK: 
		-  output: JWT
		-  User-ul a fost logat cu succes
	- 404 NOT FOUND:
		-  Detaliile de autentificare date nu au fost gasite

##### POST api/register:
- Input: username, password, email, adresa, telefon, tara, oras, cod postal
- Output:
	- 200 OK:
		-  output: blank
		- User-ul a fost inregistrat cu succes
	- 409 CONFLICT:
		- Exista deja un user cu acel email

### api/products:

##### GET /:
- Input: JWT
- Output:
	- 200 OK:
		- output: list of products
		- pentru admin au si campul "achizitii" inclus
	- 403 FORBIDDEN:
		- agentul nu are permisiuni

##### POST /:
- Input:  JWT, nume, pret, cantitate, descriere, photo
- Output:
	- 200 OK:
		- output: blank
	- 403 FORBIDDEN:
		- agentul nu are permisiuni

##### GET /{id}:
- Input: JWT
- Output:
	- 200 OK:
		- output: nume, pret, cantitate, descriere, photo
		- pentru admin: vine in raspuns si campul achizitii
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
	- 404 NOT FOUND:
		- produsul nu exista



##### PUT /{id}:
- Input:  JWT, nume, pret, cantitate, descriere, photo
- Output:
	- 200 OK:
		- output: blank
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
	- 404 NOT FOUND:
		- produsul nu exista

##### DELETE /{id}:
- Input:  JWT
- Output:
	- 200 OK:
		- output: blank
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
	- 404 NOT FOUND:
		- produsul nu exista


### api/users:

##### GET /:
- Input: JWT
- Output:
	- 200 OK:
		- output: list of users
		- pentru admin au si campul "achizitii" inclus
	- 403 FORBIDDEN:
		- agentul nu are permisiuni

##### POST /:
- Implementat in /api/register

##### GET /{id}:
- Input: JWT
- Output:
	- 200 OK:
		- output: username, email, adresa, telefon, tara, oras, cod postal
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
		- userul are acces doar pe el, adminul pe toti
	- 404 NOT FOUND:
		- userul nu exista



##### PUT /{id}:
- Input:  JWT, username, email, adresa, telefon, tara, oras, cod postal
- Output:
	- 200 OK:
		- output: blank
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
	- 404 NOT FOUND:
		- userul nu exista

##### DELETE /{id}:
- Input:  JWT
- Output:
	- 200 OK:
		- output: blank
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
	- 404 NOT FOUND:
		- userul nu exista



### api/orders:

##### GET /:
- Input: JWT
- Output:
	- 200 OK:
		- output: list of ordres
	- 403 FORBIDDEN:
		- agentul nu are permisiuni

##### POST /:
- Input: JWT, product_id, quantity
- Output:
	- 200 OK:
		- output: blank
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
		- userul are acces doar pe el, adminul pe toti
	- 409 CONFLICT:
		-  order-ul nu poate fi satisfacut

##### GET /{id}:
- Input: JWT
- Output:
	- 200 OK:
		- output: user_id, product_id, quanity
	- 403 FORBIDDEN:
		- agentul nu are permisiuni
	- 404 NOT FOUND:
		- comanda nu exista