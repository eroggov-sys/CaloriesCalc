import { useState } from "react"
import {
  Field,
  FieldGroup,
  FieldLabel,
  FieldSet,
} from "@/components/ui/field"
import { login } from "@/api/auth"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"

const LoginForm = ({ onLogin, onShowRegister }) => {

    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [error, setError] = useState("")
    const [isLoading, setIsLoading] = useState(false)


    async function handleSubmit(event) {
        event.preventDefault()

        try {
            setIsLoading(true)
            setError("")

            await login(email, password)
            onLogin()
        } catch (requestError) {
            setError(requestError.message)
        } finally {
            setIsLoading(false)
        }
    }

    return (
  <form onSubmit={handleSubmit} className="w-full max-w-xs">

    <div className="space-y-1 pb-4">
        <h1 className="text-2xl font-bold text-zinc-950">
            Sign in
        </h1>

        <p className="text-sm text-zinc-500">
            Enter your account details.
        </p>
    </div>

    <FieldSet>
      <FieldGroup>
        <Field>
          <FieldLabel htmlFor="email">Email</FieldLabel>

          <Input
            id="email"
            type="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            placeholder="user@example.com"
            required
          />
        </Field>

        <Field>
          <FieldLabel htmlFor="password">Password</FieldLabel>

          <Input
            id="password"
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="Enter your password"
            required
          />
        </Field>

        {error && (
          <p className="text-sm text-red-600">
            {error}
          </p>
        )}

        <Button type="submit" disabled={isLoading}>
          {isLoading ? "Signing in..." : "Sign in"}
        </Button>

        <Button
            type="button"
            variant="link"
            onClick={onShowRegister}
        >
            Create an account
        </Button>

      </FieldGroup>
    </FieldSet>
  </form>
)
}

export default LoginForm