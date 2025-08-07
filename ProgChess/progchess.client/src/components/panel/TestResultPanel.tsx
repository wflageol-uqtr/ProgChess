import { CheckLine, X } from "lucide-react";
import type { TestResult } from "../../utils/type";
import { useState } from "react";

interface TestResultPanelProps {
  isPending: boolean;
  testResult: TestResult[];
}

export default function TestResultPanel({
  testResult,
  isPending,
}: TestResultPanelProps) {
  const [selectedIndex, setSelectedIndex] = useState(0);

  return (
    <>
      {testResult.length > 0 ? (
        <>
          <div className="flex justify-end mt-1">
            <div className="flex items-center gap-3 px-3 py-2 bg-zinc-700 rounded-lg text-sm">
              <span className="text-zinc-300">({testResult.length} tests)</span>

              <div className="flex items-center gap-1">
                <CheckLine className="w-4 h-4 text-green-500" />
                <span className="text-green-400">
                  {
                    testResult.filter((value) => value.$type === "success")
                      .length
                  }
                </span>
              </div>

              <div className="flex items-center gap-1">
                <X className="w-4 h-4 text-red-500" />
                <span className="text-red-400">
                  {" "}
                  {testResult.filter((value) => value.$type === "error").length}
                </span>
              </div>
            </div>
          </div>
          <div className="flex mt-6 mx-6 rounded-2xl overflow-auto border border-zinc-700 shadow-md">
            <div className="w-1/2 bg-zinc-800 p-4 space-y-2 overflow-auto max-h-72">
              {testResult.map((value, index) => (
                <div
                  key={index}
                  onClick={() => setSelectedIndex(index)}
                  className={`bg-zinc-600 p-4 rounded-xl shadow-sm border-l-4 transition-all duration-150 cursor-pointer hover:bg-zinc-500 ${
                    value.success ? "border-green-600" : "border-red-600"
                  }`}
                >
                  <h2 className="text-base font-semibold text-zinc-100 mb-1">
                    {value.testName}
                  </h2>
                  <p className="text-sm text-zinc-300">
                    Statut :{" "}
                    <span
                      className={`font-medium ${
                        value.success ? "text-green-400" : "text-red-400"
                      }`}
                    >
                      {value.success ? "Réussie" : "Échoué"}
                    </span>
                  </p>
                </div>
              ))}
            </div>

            <div className="w-1/2 bg-zinc-900 p-6 overflow-y-auto text-sm text-zinc-200">
              {selectedIndex !== null && (
                <div className="space-y-4">
                  <div className="text-lg font-semibold text-zinc-100">
                    {testResult[selectedIndex].testName}
                  </div>

                  {testResult[selectedIndex].actual && (
                    <div className="space-y-2">
                      <div>
                        <div className="text-xs text-zinc-400 mb-1">
                          Actuel:
                        </div>
                        <pre className="bg-zinc-800 p-3 rounded-lg whitespace-pre-wrap break-words">
                          {testResult[selectedIndex].actual}
                        </pre>
                      </div>

                      {testResult[selectedIndex].expected && (
                        <div>
                          <div className="text-xs text-zinc-400 mb-1">
                            Attendu:
                          </div>
                          <pre className="bg-zinc-800 p-3 rounded-lg whitespace-pre-wrap break-words">
                            {testResult[selectedIndex].expected}
                          </pre>
                        </div>
                      )}
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
        </>
      ) : (
        <>
          {isPending ? (
            <div className="flex w-full h-64 justify-center items-center">
              <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-white"></div>
            </div>
          ) : (
            <div className="flex w-full h-64 justify-center items-center text-zinc-400">
              Exécuter les tests pour voir une sortie
            </div>
          )}
        </>
      )}
    </>
  );
}
