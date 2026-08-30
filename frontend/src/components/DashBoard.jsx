import Macronoutrients from "./Macronutrients"
import Meals from "./Meals"
import { useEffect, useState } from "react"
import { getDailyNutrition } from "@/api/foodEntries"
import { LogOut, UserRoundPen } from "lucide-react"
import { Button } from "@/components/ui/button"
import { getNutritionTargets } from "@/api/profile"

function getTodayDate() {
  const now = new Date()
  const timezoneOffset = now.getTimezoneOffset() * 60_000
  const localDate = new Date(now.getTime() - timezoneOffset)

  return localDate.toISOString().split("T")[0]
}

export default function DashBoard({ onLogout, onEditProfile }) {

  const [nutrition, setNutrition] = useState(null)
  const [error, setError] = useState("")
  const [selectedDate, setSelectedDate] = useState(getTodayDate)
  const [refreshKey, setRefreshKey] = useState(0)
  const [targets, setTargets] = useState(null)
  const [isTargetsLoading, setIsTargetsLoading] = useState(true)

  useEffect(() => {
    async function loadNutrition () {

      try {
        const data = await getDailyNutrition(selectedDate)
        setNutrition(data)
      } 
      catch (requestError) { 
        setError(requestError.message)
      }
    }
    loadNutrition()
  },[selectedDate, refreshKey])

  useEffect(() => {
    let canceled =false
    async function loadTargets() {

      try {
        setIsTargetsLoading(true)

        const data = await getNutritionTargets()

        if (!canceled) setTargets(data)

      } catch (error) {
        
        if (!canceled) setError(error.message)
      } finally {

        if (!canceled) setIsTargetsLoading(false)
      } 
    }
    loadTargets()

    return () => canceled = true
  }, [])

  if (isTargetsLoading) {
    return(
      <main className="grid min-h-screen place-items-center">
        <p className="text-sm text-zinc-500">
          Loading nutrition targets...
        </p>
    </main>
    )
  }


  if (error) {
    return <p>Failed to load nutrition: {error}</p>
  } 

  return (
    <main className="min-h-screen bg-zinc-50 px-4 py-6 sm:px-6">
      
      <div className="mx-auto w-full max-w-5xl space-y-6 ">
        <div className="flex items-center justify-between gap-3">
          <input
            type="date"
            value={selectedDate}
            onChange={(event) => setSelectedDate(event.target.value)}
            className="h-10 rounded-md border border-zinc-300 bg-white px-3"
          />
          <div className="flex gap-2 ">
            <Button
            type="button"
            variant="outline"
            size="icon"
            onClick={onEditProfile}
            aria-label="Edit profile"
            title="Edit profile"
          >
            <UserRoundPen className="size-4" />
          </Button>


          <Button
            type="button"
            variant="outline"
            size="icon"
            onClick={onLogout}
            aria-label="Sign out"
            title="Sign out"
          >
            <LogOut className="size-4" />
          </Button>
          </div>
          
      </div>
        <Macronoutrients title={`Calories`} consumed={nutrition?.calories ?? 0} goal={targets?.calories ?? 0} currency={"kcal"} />


        <div className="grid grid-cols-1 gap-3 lg:grid-cols-4" >

          <Macronoutrients title={`Protein`} consumed={nutrition?.protein ?? 0} goal={targets?.protein ?? 0} currency={"g"} />
          <Macronoutrients title={`Fat`} consumed={nutrition?.fat ?? 0} goal={targets?.fat ?? 0} currency={"g"} />
          <Macronoutrients title={`Carbs`} consumed={nutrition?.carbs ?? 0} goal={targets?.carbs ?? 0} currency={"g"} />
          <Macronoutrients title={`Sugar`} consumed={nutrition?.sugar ?? 0} goal={targets?.sugarLimit ?? 0} currency={"g"} />


        </div>
        <Meals 
        date={selectedDate}
        refreshKey = {refreshKey}
        onEntryCreated= {() => setRefreshKey((value) => value + 1)}
        />
      </div>
    </main>
  )
}

