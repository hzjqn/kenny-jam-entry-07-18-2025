import random
import functools


class GameManager:
	mazo_personajes = []
	# Cartas por palo:
	# Truco 10
	# Baraja española 12
	# Tarot 14
	# Poker 13
	# NuevoMazo 11

	PALOS = "eboc"
	CARTAS_POR_PALO = 3
	#cartas_por_palo = 11
	def build_mazo_personajes():
		mazo = []
		for valor in range(GameManager.CARTAS_POR_PALO):
			for palo in GameManager.PALOS:
				#if valor == 1 and palo == "e": #sacando el rey.
					#continue
				#else:
					#for i in range(valor):
				mazo.append([valor,palo])
		return mazo
	

	def build_mazo_habilidades():
		#1 para convertir a cada palo.
		#2 50% de chances para duplicar valor (Coinflip) (Tweakable: O se destruye la carta)
		mazo = []

		def convertir(carta,palo):
			carta[1] = palo
		def flip(carta):
			heads_or_tails = random.randrange(2)
			if random.randrange(2) == 1:
				carta[0] = carta[0]*2
			return heads_or_tails


		for palo in GameManager.PALOS:
			#lambda(target) target puede ser None, 1 carta o 2)
			#(argc, func) 
			mazo.append(("convertir_"+"palo",1,functools.partial(convertir,palo=palo)))
		mazo.append(("flip",1,flip))
		return mazo
					



	#popularidades = {palo:0 for palo in GameManager.PALOS}

	#popularidad = 0
	#alianzas = [0]*16

	def diezmo (x):
		return 0.1*x

	#GAMESTATE/ESTADO
	def __init__(self):
		self.monedas = 20
		self.fuerza = 20
		#self.s = [0]*len(PALOS)
		self.popularidad = 0
		self.gasto = 1
		self.mazo_personajes = GameManager.build_mazo_personajes()
		self.mazo_habilidades = GameManager.build_mazo_habilidades()

	def pagar_diezmo(self):
		self.monedas = self.monedas * 0.9

	def pagar_impuestos(self):
		self.monedas = self.monedas * 0.7 #rey no paga impuestos


	def cobrar_impuestos(self):
		self.monedas = self.monedas + 1 #reemplazar por funcion de cartas en deck


	ACCIONES = ("IGNORAR","VIOLENCIA","PAGAR","COBRAR") # 0,-2,-1,+1
	
	def ignorar(self,carta):
		self.popularidad -=1
	
	def pagar(self,carta):
		palo = carta[1]
		valor = carta[0]
		if palo == "c":
			self.monedas = int(0.9*self_monedas) # pagar_diezmo
			self.popularidad +=1
			#self.popularidades[palo] +=1
			#self.popularidades["e"] +=1
			#self.popularidades["c"] +=1
			#self.popularidades["b"] +=1
			#fe +=1


		if palo == "e":
			self.monedas -=3*valor
			#self.popularidades[palo] +=1
			self.popularidad+=1
			self.fuerza+=1
		if palo == "b":
			self.monedas -=1*valor
			self.popularidad-=1
			#self.popularidades[palo] +=1

		if palo == "o":
			self.monedas -=2*valor
			self.popularidad +=1
			#self.popularidades[palo] +=1
			#self.popularidades["b"]  -=1
			#self.popularidades["e"] -=1
			#self.popularidades["c"] -=1

	def cobrar(self,carta):
		palo = carta[1]
		valor = carta[0]
		if palo =="c":
			self.popularidad-= 3
			self.monedas+= 1*valor
		if palo == "e":
			self.popularidad-= 3
			self.monedas+= 1*valor
		if palo == "b":
			self.popularidad-1
			self.popularidad+=2
		if palo == "o":
			self.popularidad+=1
			self.gasto+=1


	def violencia(self,carta):
		palo = carta[1]
		valor = carta[0]
		if palo =="c":
			self.popularidad-= 3
			self.monedas+= 1*valor
		if palo == "e":
			self.popularidad+= 3
			self.monedas+= 1*valor
		if palo == "b":
			self.monedas +=1
			self.popularidad+=3
			self.fuerza-=3
		if palo == "o":
			self.popularidad+=2
			self.monedas+=2*valor
			self.gasto+=1

	def popularidad():
		return 	functools.reduce(int.__add__,popularidades)
	
	



	def print_state(self):
		print("deck_size",len(self.mazo_personajes))
		print("monedas",self.monedas)
		#print("vida",self.vida)
		print("popularidad",self.popularidad)
		print("gasto",self.gasto)
		print("fuerza",self.fuerza)
		print("people in room",self.people_room)
		print("Actions available",self.actions_room)
		


	def robar_personaje(self):
		i = random.randrange(len(self.mazo_personajes))
		return self.mazo_personajes.pop(i)
	
	def robar_habilidad(self):
		i = random.randrange(len(self.mazo_habilidades))
		return self.mazo_habilidades.pop(i)
	CARDS_PER_ROOM = 4
	SKILLS_PER_ROOM = 2
	def main(self):
		


		n_cartas = 0
		while self.fuerza>0 and len(self.mazo_personajes)+ n_cartas>0:
			if n_cartas ==0:
				n_cartas = 4
				self.people_room = {i:self.robar_personaje() for i in range(GameManager.CARDS_PER_ROOM)}
				self.actions_room = { 0:("ignorar",1,self.ignorar), \
						 1:("pagar",1,self.pagar), \
						 2:("cobrar",1,self.cobrar), \
						 3:("violencia",1,self.violencia) }
				
				self.actions_room.update( { GameManager.CARDS_PER_ROOM + i: self.robar_habilidad() if len(self.mazo_habilidades) >0 else None for i in range(GameManager.SKILLS_PER_ROOM) } )
				n_cartas = GameManager.CARDS_PER_ROOM
			self.print_state()
			action_i = input("Choose action (0 to 3 for basics, 4 and 5 for specials.)")
			card_i = input("Choose card(0 to 3)")
			#TODO: Multiple targets
			
			#performing action
			action = self.actions_room[int(action_i)]
			action_name = action[0]
			action_function = action[2]
			print("DEBUG",action_function)
			action_function( self.people_room[int(card_i)] )
			self.actions_room[int(action_i)] = None
			if action_i in "0123":
				self.people_room[int(card_i)] = None
				n_cartas-=1
		
		
			 

gm = GameManager()
gm.main()


		
