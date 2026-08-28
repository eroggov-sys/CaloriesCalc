import { Card, CardContent, CardHeader, CardTitle, } from "@/components/ui/card"

export default function Macronoutrients({title, consumed, goal, currency}) {

    const percent = Math.min(Math.round((consumed / goal) * 100), 100)
    const remaining = Math.max(goal - consumed, 0)

    const radius = 30
    const circumference = 2 * Math.PI * radius
    const circleOffset = circumference * (1 - percent / 100)

  return (
    <Card className=" w-full rounded-2xl border-zinc-200 shadow-sm">
      <CardHeader className="px-3">
        <CardTitle className="text-xs font-medium text-zinc-600 sm:text-sm">
            {title}
        </CardTitle>
      </CardHeader>

      <CardContent className="px-3">
        <div className="flex items-center justify-between">
          <div className="flex items-baseline gap-1">
            <span className="text-2xl font-bold tracking-tight text-zinc-950">
              {consumed}
            </span>

            <span className="text-sm font-semibold text-zinc-800">
              / {`${goal} ${currency}`} 
            </span>
          </div>

          <div className="hidden relative size-[72px] sm:block">
            <svg
              viewBox="0 0 72 72"
              className="size-full -rotate-90 "
              aria-label={`${percent}% complete`}
            >
              <circle
                cx="36"
                cy="36"
                r={radius}
                fill="none"
                stroke="#e5e7eb"
                strokeWidth="5"
              />

              <circle
                cx="36"
                cy="36"
                r={radius}
                fill="none"
                stroke="#36ad62"
                strokeWidth="5"
                strokeLinecap="round"
                strokeDasharray={circumference}
                strokeDashoffset={circleOffset}
                className="transition-all duration-500"
              />
            </svg>

            <span className="absolute inset-0 flex items-center justify-center text-sm font-medium text-zinc-600">
              {percent}%
            </span>
          </div>
        </div>

        <div className="mt-4 h-1.5 overflow-hidden rounded-full bg-zinc-200">
          <div
            className="h-full rounded-full bg-[#36ad62] transition-all duration-500"
            style={{ width: `${percent}%` }}
          />
        </div>

        <p className="mt-2 text-sm text-zinc-600">
            {`${remaining} ${currency}`} left
        </p>
      </CardContent>
    </Card>
  )
  
}

