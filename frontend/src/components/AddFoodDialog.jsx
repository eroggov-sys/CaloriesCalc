import { useEffect, useState } from "react"
import { searchFoods } from "@/api/foods"
import { Input } from "./ui/input"
import { Plus } from "lucide-react"
import { Button } from "@/components/ui/button"
import {Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle, DialogTrigger, } from "@/components/ui/dialog"
import { createFoodEntry } from "@/api/foodEntries"
import { Select,SelectTrigger, SelectValue, SelectContent, SelectGroup, SelectLabel, SelectItem, } from "./ui/select"




const AddFoodDialog = ({date, onCreated, }) => {
    const [query, setQuery] = useState("")
    const [foods, setFoods] = useState([])
    const [selectedFood, setSelectedFood] = useState(null)
    const [isSearching, setIsSearching] = useState(false)
    const [error, setError] = useState("")
    const [open, setOpen] = useState(false)

    const [quantityGrams, setQuantityGrams] = useState("")
    const [mealType, setMealType] = useState("Breakfast")
    const [isSaving, setIsSaving] = useState(false)
    

    const mealTypes = [ 
        { label: "Breakfast", value: "Breakfast" },
        { label: "Lunch", value: "Lunch" },
        { label: "Dinner", value: "Dinner" },
        { label: "Snacks", value: "Snacks" }
    ]
    
    useEffect(() => {
        const searchQuery = query.trim();

        if (searchQuery.length < 2 || selectedFood?.name === query){ 
            return
        }

        let canceled= false 

        const timeoutId = setTimeout(async () => {
            setIsSearching(true)
            setError("")

            try {

                const result = await searchFoods(searchQuery)
                if (!canceled) setFoods(result)

            } catch (requestError) {
                if(!canceled) {
                    setError(requestError.message)
                    setFoods([])
                }
            } finally {
                if(!canceled) setIsSearching(false)
            }
        }, 350)

        return () => {
            canceled = true
            clearTimeout(timeoutId)
        }

    },[query, selectedFood])


    const handleSelectFood = (food) =>{
        setSelectedFood(food)
        setQuery(food.name)
        setFoods([])
    }   

    async function handleSubmit(event) {
        event.preventDefault()

        if(!selectedFood)  {
            setError("Chose product")
            return
        }

        const quantity = Number(quantityGrams)

        if (!Number.isFinite(quantity) || quantity <= 0) {
            setError("Enter correct quantity")
            return
        }

        setIsSaving(true)
        setError("")

        try {
            await createFoodEntry(selectedFood.id, {
                quantityGrams : quantity,
                eatenAt : `${date}T12:00:00Z`,
                mealType,
            })

            setOpen(false)
            setQuery("")
            setSelectedFood(null)
            setQuantityGrams("")

            onCreated?.()
        } catch (requestError) {
            setError(requestError.message)
        } finally {
            setIsSaving(false)
        }
    }

    return(
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger
                render={
                <Button className="bg-green-600 text-white hover:bg-green-700" />
                }
            >
                <Plus className="size-4" />
                Add food
            </DialogTrigger>
            
            <DialogContent>
                <form onSubmit={handleSubmit}> 
                    <DialogHeader>
                    <DialogTitle>Add food</DialogTitle>

                    <DialogDescription>
                        Find a food and add it to your diary.
                    </DialogDescription>
                    </DialogHeader>

            <div className="relative w-full max-w-sm">
                <Input 
                    value={query}
                    placeholder="Enter product's name"
                    onChange = {(event) => {
                        setQuery(event.target.value)
                        setSelectedFood(null)
                        setFoods([])
                        setError("")
                    }}
                />
                <Input
                    type="number"
                    min="0"
                    step="1"
                    value={quantityGrams}
                    placeholder="Quantity, g"
                    onChange={(event) => setQuantityGrams(event.target.value)}
                />
                
                <Select 
                items={mealTypes}
                value={mealType} 
                onValueChange={ setMealType }
                >
                <SelectTrigger className="w-full max-w-48">
                    <SelectValue />
                </SelectTrigger>
                <SelectContent>
                    <SelectGroup>
                    <SelectLabel>Meal</SelectLabel>
                    {mealTypes.map((item) => (
                        <SelectItem key={item.value} value={item.value}>
                        {item.label}
                        </SelectItem>
                    ))}
                    </SelectGroup>
                </SelectContent>
                </Select>



                {isSearching && (<p className="mt-2 text-sm text-zinc-500">Searching...</p>)}

                {error && (
                    <p className="mt-2 text-sm text-red-600">
                    {error}
                    </p>
                )}

                {foods.length > 0 && (
                    <div className="absolute z-20 mt-1 max-h-64 w-full overflow-y-auto rounded-md border border-zinc-200 bg-white shadow-lg">
                    {foods.map((food) => (
                        <button
                        key={food.id}
                        type="button"
                        onClick={() => handleSelectFood(food)}
                        className="flex w-full items-center justify-between gap-4 px-3 py-2 text-left hover:bg-zinc-50"
                        >
                        <span>{food.name}</span>

                        <span className="whitespace-nowrap text-sm text-zinc-500">
                            {food.caloriesPer100g} kcal / 100 g
                        </span>
                        </button>
                    ))}
                    </div>
                )}

            </div>
            <Button 
                type="submit"
                disabled={isSaving || !selectedFood}
            >
                {isSaving ? "Saving..." : "Add"}
            </Button>

            </form>
        </DialogContent>
    </Dialog>
        
    )
}

export default AddFoodDialog
