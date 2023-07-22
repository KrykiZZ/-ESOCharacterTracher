KKZCT = {
    name = "CharactersTracker",
    logger = nil
}

-- BEGIN: Mount Leveling.
function KKZCT.OnRidingSkillImprovement(ridingSkill, previous, current, source)
    KKZCT.SaveMountData(true)
end

function KKZCT.SaveMountData(isTrain)
    local capacityBonus, maxCapacityBonus, staminaBonus, maxStaminaBonus, speedBonus, maxSpeedBonus = GetRidingStats()
    
    KKZCT.savedVariables.capacityBonus = capacityBonus
    KKZCT.savedVariables.maxCapacityBonus = maxCapacityBonus

    KKZCT.savedVariables.staminaBonus = staminaBonus
    KKZCT.savedVariables.maxStaminaBonus = maxStaminaBonus

    KKZCT.savedVariables.speedBonus = speedBonus
    KKZCT.savedVariables.maxSpeedBonus = maxSpeedBonus

    if isTrain == true then
        KKZCT.savedVariables.lastMountTrain = os.date("%H:%M:%S %d.%m.%Y")
    end
end
-- END: Mount Leveling.

-- BEGIN: Crafting Leveling,
function KKZCT.OnSkillLineXPUpdated(skillLineData) KKZCT.SaveCraftingRankData() end

function KKZCT.SaveCraftingRankData(skillLineData)
    local id = skillLineData:GetId()
    local rank = skillLineData:GetCurrentRank()

    if id == 77 then KKZCT.savedVariables.alchemyRank = rank
    elseif id == 78 then KKZCT.savedVariables.enchantingRank = rank
    elseif id == 79 then KKZCT.savedVariables.blacksmithingRank = rank
    elseif id == 81 then KKZCT.savedVariables.clothingRank = rank
    elseif id == 76 then KKZCT.savedVariables.provisioningRank = rank
    elseif id == 80 then KKZCT.savedVariables.woodworkingRank = rank
    elseif id == 141 then KKZCT.savedVariables.jewelryRank = rank
    end
end
-- END: Crafting Leveling.

-- BEGIN: Currency.
function KKZCT.SaveCurrency()
    KKZCT.savedVariables.gold = GetCarriedCurrencyAmount(CURT_MONEY)
    KKZCT.savedVariables.alliancePoints = GetCarriedCurrencyAmount(CURT_ALLIANCE_POINTS)
    KKZCT.savedVariables.telvar = GetCarriedCurrencyAmount(CURT_TELVAR_STONES)
    KKZCT.savedVariables.vouchers = GetCarriedCurrencyAmount(CURT_WRIT_VOUCHERS)
end
-- END: Currency.

function KKZCT.Initialize()
    KKZCT.logger = LibDebugLogger(KKZCT.name)

    KKZCT.savedVariables = ZO_SavedVars:NewCharacterIdSettings("CharactersTracker_Data", 1, GetWorldName(), {})
    KKZCT.SaveMountData(false)

    -- BEGIN: Mount Leveling.
    EVENT_MANAGER:RegisterForEvent(KKZCT.name, EVENT_RIDING_SKILL_IMPROVEMENT, KKZCT.OnRidingSkillImprovement)
    -- END: Mount Leveling.

    -- BEGIN: Crafting Leveling.
    SKILLS_DATA_MANAGER:RegisterCallback("SkillLineXPUpdated", KKZCT.OnSkillLineXPUpdated)

    for i = 1, GetNumSkillLines(SKILL_TYPE_TRADESKILL) do
        local skillLineData = SKILLS_DATA_MANAGER:GetSkillLineDataByIndices(SKILL_TYPE_TRADESKILL, i)
        KKZCT.SaveCraftingRankData(skillLineData)
    end
    -- END: Crafting Leveling.

    -- BEGIN: Daily LFG.
    KKZCT.savedVariables.dailyDungeonRemainingTime = GetLFGCooldownTimeRemainingSeconds(LFG_COOLDOWN_DUNGEON_REWARD_GRANTED)
    KKZCT.savedVariables.dailyDungeonRemainingTime = GetLFGCooldownTimeRemainingSeconds(LFG_COOLDOWN_BATTLEGROUND_REWARD_GRANTED)
    -- END: Daily LFG.

    -- BEGIN: Currency.
    KKZCT.SaveCurrency()
    -- END: Currency.
end

function KKZCT.OnAddOnLoaded(event, addonName)
	if addonName == KKZCT.name then
        KKZCT.Initialize()
        EVENT_MANAGER:UnregisterForEvent(KKZCT.name, EVENT_ADD_ON_LOADED) 
    end
end

EVENT_MANAGER:RegisterForEvent(KKZCT.name, EVENT_ADD_ON_LOADED, KKZCT.OnAddOnLoaded)