import {  useState } from "react"

import { deleteFoodEntry, } from "@/api/foodEntries"
import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"


const DeleteFoodEntryDialog = ({ entry, open, onOpenChange, onDeleted }) => {

    const [error, setError] = useState("")
  
    async function handleDelete(event) {
        event.preventDefault()

        try {
            setError("")

            await deleteFoodEntry(entry.id)

            onOpenChange(false)
            onDeleted?.()

        } catch (requestError) {
            
            setError(requestError.message)
        }
    }

    return (
  <Dialog open={open} onOpenChange={onOpenChange}>
    <DialogContent>
      <form onSubmit={handleDelete} className="grid gap-4">
        <DialogHeader>
          <DialogTitle>Delete food</DialogTitle>

          <DialogDescription>
            Delete {entry.foodName}? This action cannot be undone.
          </DialogDescription>
        </DialogHeader>

        {error && (
          <p className="text-sm text-red-600">
            {error}
          </p>
        )}

        <div className="flex justify-end gap-2">
          <Button
            type="button"
            variant="outline"
            onClick={() => onOpenChange(false)}
          >
            Cancel
          </Button>

          <Button type="submit" variant="destructive">
            Delete
          </Button>
        </div>
      </form>
    </DialogContent>
  </Dialog>
)
}

export default DeleteFoodEntryDialog