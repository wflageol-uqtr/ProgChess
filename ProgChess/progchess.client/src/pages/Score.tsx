import { useLocation } from "react-router";
import type { TestResult } from "../utils/type";

export default function Score() {
  const { state } = useLocation();
  const { score, testResult } = state;

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-zinc-900 text-white p-6">
      <div className="bg-zinc-800 p-8 rounded-2xl shadow-lg w-full max-w-md text-center">
        <h1 className="text-3xl font-bold mb-4">🎯 Résultat du test</h1>
        <p className="text-xl mb-6">
          Score :{" "}
          <span className="font-bold">
            {score} / {testResult.value.length}
          </span>
        </p>

        <ul className="space-y-2 text-left">
          {testResult.value.map((test: TestResult, index: number) => (
            <li
              key={index}
              className={`p-3 rounded-lg flex items-center justify-between ${
                test.success ? "bg-green-600" : "bg-red-600"
              }`}
            >
              <span>{test.testName}</span>
              <span>{test.success ? "✅ Réussi" : "❌ Échoué"}</span>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
