import { useEffect, useState, useRef } from "react"
import { searchFoods,  importFood, findFoodByBarcode } from "@/api/foods"
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
    const [hasSearched, setHasSearched] = useState(false)
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

    const [isSearchingExternal, setIsSearchingExternal] = useState(false)


    const [page, setPage] = useState(1)
    const [hasMore, setHasMore] = useState(false)
    const [isLoadingMore, setIsLoadingMore] = useState(false)
    const latestQueryRef = useRef("")

    const [isImporting, setIsImporting] = useState(false)

    const [barcode, setBarcode] = useState("")
    const [isLookingUpBarcode, setIsLookingUpBarcode] = useState(false)

    
    
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
                if (!canceled) {
                    setFoods(result.items)
                    setHasMore(result.hasMore)
                    setPage(1)
                    setHasSearched(true)
                }

            } catch (requestError) {
                if(!canceled) {
                    setError(requestError.message)
                    setFoods([])
                    setHasSearched(false)
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


    async function handleSelectFood(food) {
        setFoods([])
        setHasSearched(false)
        setHasMore(false)
        setQuery(food.name)

        if (food.id) {
            setSelectedFood(food)
            return
        }

        setIsImporting(true)
        setError("")

        try {
            const importedFood = await importFood(food.source, food.externalId)
            setSelectedFood(importedFood)
        } catch (requestError) {
            setError(requestError.message)
            setSelectedFood(null)
        } finally {
            setIsImporting(false)
        }
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
            await createFoodEntry({
                foodId: selectedFood.id,
                quantityGrams : quantity,
                date,
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

    async function handleSearchExternal() {
        const searchQuery = query.trim()

        setIsSearchingExternal(true)
        setError("")

        try {
            const result = await searchFoods(searchQuery, { external: true })
            setFoods(result.items)
            setHasMore(result.hasMore)
            setPage(1)
            setHasSearched(true)

        } catch (requestError) {
            setError(requestError.message)
        } finally {
            setIsSearchingExternal(false)
        }
    }

    async function handleBarcodeLookup() {
        const code = barcode.trim()

        if (!/^\d{8,14}$/.test(code)) {
            setError("Barcode must contain 8 to 14 digits")
            return
        }

        setIsLookingUpBarcode(true)
        setError("")

        try {
            const food = await findFoodByBarcode(code)

            if (food === null) {
                setError("Product with this barcode was not found")
                return
            }

            setSelectedFood(food)
            setQuery(food.name)
            setFoods([])
            setHasSearched(false)
        } catch (requestError) {
            setError(requestError.message)
        } finally {
            setIsLookingUpBarcode(false)
        }
    }


    async function handleLoadMore() {
        const searchQuery = query.trim()
        const nextPage = page + 1

        setIsLoadingMore(true)

        try {
            const result = await searchFoods(searchQuery, { page: nextPage })

            if (latestQueryRef.current !== searchQuery) return

            setFoods((previous) => {
                const knownIds = new Set(previous.map((food) => food.id))
                return [...previous, ...result.items.filter((food) => !knownIds.has(food.id))]
            })
            setHasMore(result.hasMore)
            setPage(nextPage)
        } catch (requestError) {
            setError(requestError.message)
        } finally {
            setIsLoadingMore(false)
        }
    }

    const searchExternalButton = (
        <button
            type="button"
            onClick={handleSearchExternal}
            disabled={isSearchingExternal}
            className="w-full px-3 py-2 text-left text-sm text-green-700 hover:bg-zinc-50 disabled:opacity-50"
        >
            {isSearchingExternal ? "Searching Open Food Facts..." : "Search Open Food Facts"}
        </button>
    )

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
                        latestQueryRef.current = event.target.value.trim()   
                        setQuery(event.target.value)
                        setSelectedFood(null)
                        setFoods([])
                        setError("")
                        setHasSearched(false)
                        setHasMore(false) 
                    }}
                />
                
                <div className="mt-2 flex gap-2">
                    <Input
                        value={barcode}
                        inputMode="numeric"
                        placeholder="Barcode"
                        onChange={(event) => setBarcode(event.target.value)}
                    />

                    <Button
                        type="button"
                        variant="outline"
                        onClick={handleBarcodeLookup}
                        disabled={isLookingUpBarcode}
                    >
                        {isLookingUpBarcode ? "..." : "Find"}
                    </Button>
                </div>

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
                {!isSearching && hasSearched && foods.length === 0 && (
                    <div className="mt-2">
                        <p className="text-sm text-zinc-500">Nothing found locally.</p>
                        {hasMore && (
                            <button
                                type="button"
                                onClick={handleLoadMore}
                                disabled={isLoadingMore}
                                className="w-full px-3 py-2 text-center text-sm text-zinc-600 hover:bg-zinc-50 disabled:opacity-50"
                            >
                                {isLoadingMore ? "Loading..." : "Show more"}
                            </button>
                        )}
                        {searchExternalButton}
                    </div>
                )}

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
                            
                        <span className="flex min-w-0 flex-col">
                            <span className="truncate">{food.name}</span>

                            {food.brand && (
                                <span className="truncate text-xs text-zinc-500">{food.brand}</span>
                            )}
                        </span>

                        <span className="whitespace-nowrap text-sm text-zinc-500">
                            {food.caloriesPer100g} kcal / 100 g
                        </span>
                        </button>
                    ))}
                    {searchExternalButton}
                    </div>
                )}

            </div>
            <Button 
                type="submit"
                disabled={isSaving || !selectedFood || isImporting}
            >
                {isSaving ? "Saving..." : "Add"}
            </Button>

            </form>
        </DialogContent>
    </Dialog>
        
    )
}

export default AddFoodDialog
