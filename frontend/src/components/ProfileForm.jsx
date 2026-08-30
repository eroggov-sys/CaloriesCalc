import { useState } from "react"
import { updateProfile } from "@/api/profile"
import {
  Field,
  FieldGroup,
  FieldLabel,
  FieldSet,
} from "@/components/ui/field"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"

const ProfileForm = (props) =>{
const {initialProfile = null,
    onSaved,
    onCancel,
    } = props

 const [form, setForm] = useState({
    weightKg: initialProfile?.weightKg ?? "",
    heightCm: initialProfile?.heightCm ?? "",
    dateOfBirth: initialProfile?.dateOfBirth ?? "",
    biologicalSex: initialProfile?.biologicalSex ?? 1,
    activityLevel: initialProfile?.activityLevel ?? 1,
    nutritionGoal: initialProfile?.nutritionGoal ?? 2,
    bodyFatPercentage:initialProfile?.bodyFatPercentage ?? "",
  })
  const [error, setError] = useState("")
  const [isSaving, setIsSaving] = useState(false)

  function updateField(event){
    const {name, value} = event.target

    setForm((current) => ({
        ...current,
        [name]: value,

    }))
  }

  async function handleSubmit(event) {

    event.preventDefault()
    const profile = {
      weightKg: Number(form.weightKg),
      heightCm: Number(form.heightCm),
      dateOfBirth: form.dateOfBirth,
      biologicalSex: Number(form.biologicalSex),
      activityLevel: Number(form.activityLevel),
      nutritionGoal: Number(form.nutritionGoal),
      bodyFatPercentage:form.bodyFatPercentage === ""
        ? null
        : Number(form.bodyFatPercentage),
    }

    try {
        setIsSaving(true)
        setError("")

        const savedProfile = await updateProfile(profile)
        onSaved?.(savedProfile)

    } catch (error) {
        
        setError(error.message)
    
    }finally {
        
        setIsSaving(false)
    }
  }

    return (
    <form
      onSubmit={handleSubmit}
      className="w-full max-w-xl rounded-xl border bg-white p-6"
    >
      <div className="mb-6 space-y-1">
        <h1 className="text-2xl font-bold">
          Nutrition profile
        </h1>

        <p className="text-sm text-zinc-500">
          Enter your details to calculate daily targets.
        </p>
      </div>

      <FieldSet>
        <FieldGroup className="grid gap-5 sm:grid-cols-2">
          <Field>
            <FieldLabel htmlFor="weightKg">
              Weight (kg)
            </FieldLabel>

            <Input
              id="weightKg"
              name="weightKg"
              type="number"
              min="20"
              max="500"
              step="0.1"
              value={form.weightKg}
              onChange={updateField}
              required
            />
          </Field>

          <Field>
            <FieldLabel htmlFor="heightCm">
              Height (cm)
            </FieldLabel>

            <Input
              id="heightCm"
              name="heightCm"
              type="number"
              min="50"
              max="300"
              step="0.1"
              value={form.heightCm}
              onChange={updateField}
              required
            />
          </Field>

          <Field>
            <FieldLabel htmlFor="dateOfBirth">
              Date of birth
            </FieldLabel>

            <Input
              id="dateOfBirth"
              name="dateOfBirth"
              type="date"
              value={form.dateOfBirth}
              onChange={updateField}
              required
            />
          </Field>

          <Field>
            <FieldLabel htmlFor="biologicalSex">
              Biological sex
            </FieldLabel>

            <select
              id="biologicalSex"
              name="biologicalSex"
              value={form.biologicalSex}
              onChange={updateField}
              className="h-9 rounded-md bg-input/50 px-3 text-sm"
              required
            >
              <option value="1">Male</option>
              <option value="2">Female</option>
            </select>
          </Field>

          <Field>
            <FieldLabel htmlFor="activityLevel">
              Activity level
            </FieldLabel>

            <select
              id="activityLevel"
              name="activityLevel"
              value={form.activityLevel}
              onChange={updateField}
              className="h-9 rounded-md bg-input/50 px-3 text-sm"
              required
            >
              <option value="1">Sedentary</option>
              <option value="2">Lightly active</option>
              <option value="3">Moderately active</option>
              <option value="4">Very active</option>
              <option value="5">Extra active</option>
            </select>
          </Field>

          <Field>
            <FieldLabel htmlFor="nutritionGoal">
              Goal
            </FieldLabel>

            <select
              id="nutritionGoal"
              name="nutritionGoal"
              value={form.nutritionGoal}
              onChange={updateField}
              className="h-9 rounded-md bg-input/50 px-3 text-sm"
              required
            >
              <option value="1">Lose weight</option>
              <option value="2">Maintain weight</option>
              <option value="3">Gain weight</option>
            </select>
          </Field>

          <Field className="sm:col-span-2">
            <FieldLabel htmlFor="bodyFatPercentage">
              Body fat percentage (optional)
            </FieldLabel>

            <Input
              id="bodyFatPercentage"
              name="bodyFatPercentage"
              type="number"
              min="1"
              max="75"
              step="0.1"
              value={form.bodyFatPercentage}
              onChange={updateField}
            />
          </Field>
        </FieldGroup>

        {error && (
          <p className="mt-4 text-sm text-red-600">
            {error}
          </p>
        )}

        <div className="mt-6 flex gap-3">
            {onCancel && (
                <Button
                type="button"
                variant="outline"
                className="flex-1"
                onClick={onCancel}
                disabled={isSaving}
                >
                Cancel
                </Button>
            )}

            <Button
                type="submit"
                className="flex-1"
                disabled={isSaving}
            >
                {isSaving ? "Saving..." : "Save profile"}
            </Button>
        </div>
      </FieldSet>
    </form>
    )
}

export default ProfileForm