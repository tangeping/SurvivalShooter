# -*- coding: utf-8 -*-
import KBEngine
from KBEDebug import *
import GameConfigs
import random
import GameUtils

TIMER_TYPE_DESTROY = 1
TIMER_TYPE_BALANCE_MASS = 2

class Room(KBEngine.Entity):
	"""
	游戏场景
	"""
	def __init__(self):
		KBEngine.Entity.__init__(self)

		# 把自己移动到一个不可能触碰陷阱的地方
		self.position = (0.0, 0.0, 0.0)

		self.frameId = 0



		self.avatars = {}

		# 告诉客户端加载地图
		KBEngine.addSpaceGeometryMapping(self.spaceID, None, "spaces/gameMap")

		DEBUG_MSG('created space[%d] entityID = %i, res = %s.' % (self.roomKeyC, self.id, "spaces/gameMap"))

		# 让baseapp和cellapp都能够方便的访问到这个房间的entityCall
		KBEngine.globalData["Room_%i" % self.spaceID] = self.base

		# 设置房间必要的数据，客户端可以获取之后做一些显示和限制
		KBEngine.setSpaceData(self.spaceID, "GAME_MAP_SIZE",  str(GameConfigs.GAME_MAP_SIZE))
		KBEngine.setSpaceData(self.spaceID, "ROOM_MAX_PLAYER",  str(GameConfigs.ROOM_MAX_PLAYER))
		KBEngine.setSpaceData(self.spaceID, "GAME_ROUND_TIME",  str(GameConfigs.GAME_ROUND_TIME))

#		self.framePool[self.frameId].append(framedata)


	#--------------------------------------------------------------------------------------------
	#                              Callbacks
	#--------------------------------------------------------------------------------------------
	def onTimer(self, id, userArg):
		"""
		KBEngine method.
		使用addTimer后， 当时间到达则该接口被调用
		@param id		: addTimer 的返回值ID
		@param userArg	: addTimer 最后一个参数所给入的数据
		"""
		pass

	def onDestroy(self):
		"""
		KBEngine method.
		"""
		DEBUG_MSG("Room::onDestroy: %i" % (self.id))
		del KBEngine.globalData["Room_%i" % self.spaceID]


	def onDestroyTimer(self):
		DEBUG_MSG("Room::onDestroyTimer: %i" % (self.id))
		# 请求销毁引擎中创建的真实空间，在空间销毁后，所有该空间上的实体都被销毁
		self.destroySpace()


	def onEnter(self, entityCall):
		"""
		defined method.
		进入场景
		"""
		self.avatars[entityCall.id] = entityCall

		self.broadcastOther(entityCall)

		self.broadcastMe(entityCall)

		DEBUG_MSG('Room::onEnter space[%d] entityID = %i.' % (self.spaceID, entityCall.id))


	def onLeave(self, entityID):
		"""
		defined method.
		离开场景
		"""
		if entityID in self.avatars:
			del self.avatars[entityID]

		self.broadLeaveOther(entityCall)

		DEBUG_MSG('Room::onLeave space[%d] entityID = %i.' % (self.spaceID, entityID))


	def broadEnterOther(self,entityCall):

		if entityCall is None:
			return

		for e in self.avatars.values():
			if e.id == entityCall.id:
				continue
			elif e.client is not None:
				e.client.onEnterRoom(entityCall.id)

	def broadEnterMe(self,entityCall):

		if entityCall is None:
			return

		for e in self.avatars.values():
			if e.id == entityCall.id:
				continue
			elif e.client is not None:
				entityCall.client.onEnterRoom(e.id)

	def broadLeaveOther(self,entityCall):

		if entityCall is None:
			return

		for e in self.avatars.values():
			if e.id == entityCall.id:
				continue
			elif e.client is not None:
				e.client.onLeaveRoom(entityCall.id)


	def broadMessage(self,frameMessage):

		for e in self.avatars.values():
			if e is None:
				continue
			elif e.client is not None:
				e.client.onRspFrameMessage(frameMessage)

	def addFrame(self,entityCall, framedata):
		"""
		添加数据帧
		"""

		if entityCall is None :
			return

		if len(self.currFrame) <= 0 :
			self.currFrame = TFrameData().createFromDict({"frameid":self.frameId,"operation":framedata})
		else:
			self.currFrame[1].append(framedata)

		self.framePool[self.frameid] = self.currFrame

	def onUpdateBegin( self ):
		"""
		同步帧开始调用
		"""
		if len(self.currFrame) <=0 :
			return

		self.broadMessage(self.currFrame)

	def onUpdateEnd( self ):
		"""
		同步帧结束时调用
		"""
		self.frameId++
		self.currFrame = TFrameData()
		pass

