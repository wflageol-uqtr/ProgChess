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
}

export default function TestCaseCard({
  isPending,
  testResult,
  unitTestCode,
  setTestCode,
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
      name: "Modification des tests",
      isActive: false,
      component: (
        <CodeEditor
          value={unitTestCode}
          height={400}
          onChange={(e) => setTestCode(e)}
        />
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
