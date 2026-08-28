import {  useState } from "react"

import { updateFoodEntry } from "@/api/foodEntries"
import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"


const EditFoodEntryDialog = ({ entry, open, onOpenChange, onUpdated }) =>{

    const [quantityGrams, setQuantityGrams] = useState(String(entry.quantityGrams))
    const [error, setError] = useState("")
    const [isSaving, setIsSaving] = useState(false)

        async function handleSubmit(event) {
            event.preventDefault()
    
            const quantity = Number(quantityGrams)
    
            if (!Number.isFinite(quantity) || quantity <= 0) {
                setError("Enter correct quantity")
                return
            }
    
            try {
                setIsSaving(true)
                setError("")

                await updateFoodEntry(entry.id, quantity)

                onOpenChange(false)
                onUpdated?.()
                
            } catch (requestError) {
                setError(requestError.message)
            } finally {
                setIsSaving(false)
            }
        }
        
    if(!entry) return null

    return(
        <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <form onSubmit={handleSubmit} className="grid gap-4">
          <DialogHeader>
            <DialogTitle>Change quantity</DialogTitle>

            <DialogDescription>
              Update the quantity for {entry.foodName}.
            </DialogDescription>
          </DialogHeader>

          <Input
            type="number"
            min="1"
            step="1"
            value={quantityGrams}
            onChange={(event) => setQuantityGrams(event.target.value)}
            placeholder="Quantity, g"
          />

          {error && (
            <p className="text-sm text-red-600">
              {error}
            </p>
          )}

          <Button type="submit" disabled={isSaving}>
            {isSaving ? "Saving..." : "Save"}
          </Button>
        </form>
      </DialogContent>
    </Dialog>
    )

}

export default EditFoodEntryDialog