import random


class Carta():



class GameManager:
	mazo_personajes = []
	# Cartas por palo:
	# Truco 10
	# Baraja española 12
	# Tarot 14
	# Poker 13
	# NuevoMazo 11

	PALOS = "eboc"
	cartas_por_palo = 3
	#cartas_por_palo = 11
	def build_mazo_personajes():
		mazo = []
		for valor in range(cartas_por_palo):
			for palo in PALOS:
				if valor == 1 and palo == "e": #sacando el rey.
					continue
				else:
					#for i in range(valor):
					mazo.append([valor,palo])

	

	def build_mazo_habilidades(mazo):
		#1 para convertir a cada palo.
		#2 50% de chances para duplicar valor (Coinflip) (Tweakable: O se destruye la carta)
		mazo = {}
		for palo in PALOS:
			#lambda(target) target puede ser None, 1 carta o 2)
			#(argc, func) 
			mazo[palo] = (1,lambda x: x[1] = palo)
		mazo["flip"] = (1,lambda x: x[0]=x[0]*2 if random.randrange(2)==0 else x[0])
		
					



	popularidades = {palo:0 for palo in PALOS}

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
		mazo_personajes = build_mazo_personajes()
		mazo_habilidades = build_mazo_habilidades()

	def pagar_diezmo(self):
		self.monedas = self.monedas * 0.9

	def pagar_impuestos(self):
		self.monedas = self.monedas * 0.7 #rey no paga impuestos


	def cobrar_impuestos(self):
		self.monedas = self.monedas + 1 #reemplazar por funcion de cartas en deck


	ACCIONES = ("IGNORAR","VIOLENCIA","PAGAR","COBRAR") # 0,-2,-1,+1
	
	def ignorar(self.palo):
		self.popularidades[palo] -=1
	
	def pagar(self,carta):
		palo = carta[1]
		valor = carta[0]
		if palo == "c":
			self.monedas = (0.9*self_monedas)int # pagar_diezmo
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
			self.gastos+=1


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
			self.-fuerza-=3
		if palo == "o":
			self.popularidad+=2
			self.monedas+=2*valor
			self.gastos+=1

	import functoools
	def popularidad():
		return 	functools.reduce(int.__add__,popularidades)
	
	def violencia(palo):
		if palo == "e":
			
	

	mazo_habilidades = []

	CARDS_PER_ROOM = 4
	SKILLS_PER_ROOM = 2

	def print_state(self):
		print("monedas",self.monedas)
		print("vida",self.vida)
		print("popularidad",self.popularidad)
		print("gasto",self.fuerza)
		print("fuerza",self.fuerza)
		print("people in room",self.people_room)
		print("Actions available",self.actions_room)


	def main(self):
		


		n_cartas = 0
		while vida>0 and mazo_personajes > 0:
			if n_cartas ==0:
				self.people_room = {i:self.mazo_personajes.pop() for i in range(CARDS_PER_ROOM)}
				actions_room = {0:ignorar,1:pagar,2:cobrar,3:violencia}
				actions_room.update( { CARDS_PER_ROOM + i: self.mazo_habilidades.pop() for i in range(SKILLS_PER_ROOM) } )
				n_cartas = 4
			print_state(self)
			action_i = input("Choose action (1 al 4 for basics, 5 y 6 for specials.)")
			card_i = input("Choose card(1 al 4)")
			actions_room[int(action_i)](self.people_room(int(card_i)))
			if action_i in "1234":
				n_cartas-=1	
		
		
			 

gm = GameManager()
gm.main()


		
