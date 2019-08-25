Characters:Add("Karen", "ChrKaren")

Background = "BgKitchen"

startNode = Scenario:CreateNode("Karen", "Hello, Human")

okNode = Scenario:CreateNode("Karen", "That's OK. Whanna kick me?")
notOkNode = Scenario:CreateNode("Karen", "Oh noes!")

startNode:AddResponse("OK", okNode)
startNode:AddResponse("NotOk :(", notOkNode)

okNode:AddResponse("YESH", startNode)

wtfNode = Scenario:CreateNode("Karen", "Where's your human decency, degenerate?")
notOkNode:AddResponse("Yaya", wtfNode)

wtfNode:AddResponse("IDK", startNode)




