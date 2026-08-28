import {
  ChevronDown,
  Ellipsis,
  EllipsisVertical,
  Sun,
  X,
} from "lucide-react"

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"

import { Button } from "@/components/ui/button"
import { Card } from "@/components/ui/card"
import { useEffect, useState } from "react"
import { getMealGroups, } from "@/api/foodEntries"
import AddFoodDialog from "./AddFoodDialog"
import EditFoodEntryDialog from "./EditFoodEntryDialog"
import DeleteFoodEntryDialog from "./DeleteFoodEntryDialog"


function MealCard({ meal, onChanged }) {

  const [isOpen, setIsOpen] = useState(true)
  const [editingEntry, setEditingEntry] = useState(null)
  const [deletingEntry, setDeletingEntry] = useState(null)

  const totalCalories = meal.totalCalories

  const Icon = Sun

  return (
    <>
    <Card className="gap-0 overflow-hidden rounded-2xl border-zinc-200 py-0 shadow-none">
      <button
        type="button"
        onClick={() => setIsOpen((value) => !value)}
        className="flex w-full items-center gap-3 px-4 py-4 text-left transition-colors hover:bg-zinc-50 sm:px-5"
        aria-expanded={isOpen}
      >
        <Icon
          className="size-5 shrink-0 text-amber-500"
          strokeWidth={1.8}
        />

        <span className="font-semibold text-zinc-950">
          {meal.mealType}
        </span>

        <span className="ml-auto whitespace-nowrap font-semibold text-zinc-900">
          {totalCalories} kcal
        </span>

        <ChevronDown
          className={`size-4 text-zinc-500 transition-transform ${
            isOpen ? "rotate-180" : ""
          }`}
        />
      </button>

      {isOpen && (
        <div className="border-t border-zinc-200">
          {meal.entries.map((food) => (
            <div
              key={food.id}
              className="grid grid-cols-[minmax(0,1fr)_auto] items-center gap-x-4 px-4 py-2.5 sm:grid-cols-[minmax(0,1fr)_minmax(140px,1fr)_auto_auto] sm:px-5"
            >
              <span className="min-w-0 pl-0 text-sm font-medium text-zinc-800 sm:pl-7">
                {food.foodName}
              </span>

              <span className="hidden text-sm text-zinc-500 sm:block">
                {food.quantityGrams} g
              </span>

              <span className="whitespace-nowrap text-sm text-zinc-500">
                {food.calories} kcal
              </span>


              <DropdownMenu>

                <DropdownMenuTrigger asChild>
                  <Button
                  type="button"
                  aria-label={`Actions: ${food.foodName}`}
                  className=" rounded-md p-1 text-zinc-500 transition-colors bg-white hover:cursor-pointer hover:bg-zinc-200 hover:text-zinc-950 "
                >
                  <EllipsisVertical className="size-4" />
                </Button>
                </DropdownMenuTrigger>

                <DropdownMenuContent align="end">

                  <DropdownMenuItem onClick={() => setEditingEntry(food)} className={`hover:cursor-pointer hover:bg-zinc-200`}>
                    <span >Change quantity</span>
                  </DropdownMenuItem>

                  <DropdownMenuItem variant="destructive" onClick={() => setDeletingEntry(food)} className={`hover:cursor-pointer`}>
                    <X className="size-4" />Delete 
                  </DropdownMenuItem>

                </DropdownMenuContent>
              </DropdownMenu>

              <span className="col-span-2 mt-0.5 text-xs text-zinc-400 sm:hidden">
                {food.quantityGrams} g
              </span>
            </div>
          ))}
        </div>
      )}
    </Card>

    {editingEntry && (
      <EditFoodEntryDialog
        key={editingEntry.id}
        entry={editingEntry}
        open={true}
        onOpenChange={(open) => {
          if (!open) setEditingEntry(null)
        }}
        onUpdated={onChanged}
      />
    )}
    {deletingEntry && (
      <DeleteFoodEntryDialog
        key={deletingEntry.id}
        entry={deletingEntry}
        open={true}
        onOpenChange={(open) => {
          if (!open) setDeletingEntry(null)
        }}
        onDeleted={onChanged}
      />
    )}
    </>
  )
}

export default function Meals({ date, refreshKey, onEntryCreated }) {
  
  const [mealGroups, setMealGroups] = useState([])
  const [error, setError] = useState("")
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    async function loadMeals() {
      try {
        setIsLoading(true)
        setError("")

        const data = await getMealGroups(date)
        setMealGroups(data)

      } catch (requestError) {
        setError(requestError.message)
      } finally{
        setIsLoading(false)
      } 
      
    }
    loadMeals()

  }, [date, refreshKey])

  if (isLoading) {
    return <p className="text-sm text-zinc-500">Loading meals...</p>
  }

  if (error) {
    return <p className="text-sm text-red-600">{error}</p>
  }
  return (
    
    <section className=" w-full ">
      <div className="mb-4 flex items-center justify-between gap-3">
        <h2 className="text-xl font-bold tracking-tight text-zinc-950">
          Your meals
        </h2>

        
        <div className="flex items-center gap-2">
          
        <AddFoodDialog 
          date = {date}
          onCreated={onEntryCreated}
        /> 
        

          <Button
            variant="outline"
            size="icon"
            aria-label="More actions"
          >
            <Ellipsis className="size-5" />
          </Button>
        </div>
      </div>

      <div className="space-y-3">
        {mealGroups.length === 0 ? (
          <p className="py-8 text-center text-sm text-zinc-500">
            No meals added for this day.
          </p>
        ) : (
          mealGroups.map((meal) => (
            <MealCard
              key={meal.mealType}
              meal={meal}
              onChanged={onEntryCreated}
            />
          ))
        )}
      </div>
    </section>
  )
}
