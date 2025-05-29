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
          <div className="flex flex-row border mt-2 rounded-lg mx-4 h-full">
            <div className="w-1/2 bg-zinc-800 p-2 space-y-2">
              {testResult.length > 0 &&
                testResult.map((value, index) => (
                  <div
                    key={index}
                    className={`p-2 rounded cursor-pointer ${
                      selectedIndex === index
                        ? "bg-zinc-600"
                        : "bg-zinc-700 hover:bg-zinc-600"
                    }`}
                    onClick={() => setSelectedIndex(index)}
                  >
                    <span
                      className={
                        value.$type === "success"
                          ? "text-green-400"
                          : "text-red-400"
                      }
                    >
                      {value.testName}
                    </span>
                  </div>
                ))}
            </div>
            <div className="w-1/2 bg-zinc-900 p-4 rounded-r-2xl text-sm text-zinc-200">
              {selectedIndex !== null && (
                <div>
                  <div className="font-semibold mb-2">
                    {testResult[selectedIndex].testName}
                  </div>
                  {testResult[selectedIndex].actual && (
                    <>
                      <pre className="whitespace-pre-wrap break-words">
                        {testResult[selectedIndex].actual}
                      </pre>
                      <pre className="whitespace-pre-wrap break-words">
                        {testResult[selectedIndex].expected}
                      </pre>
                    </>
                  )}
                </div>
              )}
            </div>{" "}
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
