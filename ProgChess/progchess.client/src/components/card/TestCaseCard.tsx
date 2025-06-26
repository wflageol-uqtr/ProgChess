import { PanelRightClose } from "lucide-react";
import type { TabType, TestResult } from "../../utils/type";
import { Tabs } from "../tabs/Tabs";
import TestResultPanel from "../panel/TestResultPanel";
import CodeEditor from "../form/input/CodeEditor";

interface TestCaseCardProps {
  isPending: boolean;
  testResult: TestResult[];
  unitTestCode: string;
  setTestCode: React.Dispatch<React.SetStateAction<string>>;
  executionError?: string;
}

export default function TestCaseCard({
  isPending,
  testResult,
  unitTestCode,
  setTestCode,
  executionError,
}: TestCaseCardProps) {
  const tabs: TabType[] = [
    {
      id: "result",
      name: "Résultat",
      isActive: true,
      component: (
        <TestResultPanel isPending={isPending} testResult={testResult} />
      ),
    },
    {
      id: "tests",
      name: "Test unitaire",
      isActive: false,
      component: (
        <CodeEditor
          value={unitTestCode}
          height={400}
          onChange={(e) => setTestCode(e)}
        />
      ),
    },
    {
      id: "errors",
      name: "Erreur d'exécution",
      isActive: false,
      component: (
        <>
          {executionError ? (
            <>
              <h3 className="text-lg font-semibold text-red-500 mb-2">
                Erreur lors de l'exécution
              </h3>
              <p className="text-sm text-red-600 leading-relaxed">
                {executionError}
              </p>
            </>
          ) : (
            <div className="flex w-full h-64 justify-center items-center text-zinc-400">
              Exécuter les tests pour voir une sortie
            </div>
          )}
        </>
      ),
    },
  ];

  return (
    <div className="bg-zinc-800 rounded-2xl h-full flex flex-col overflow-auto">
      <div className="flex justify-between w-full p-2 items-center bg-zinc-700 rounded-t-2xl">
        <div className="flex gap-2 items-center">
          <PanelRightClose className="text-green-500" />
          <h4 className="font-semibold text-xl">Résultats des tests</h4>
        </div>
      </div>
      <Tabs tabs={tabs} />
    </div>
  );
}
