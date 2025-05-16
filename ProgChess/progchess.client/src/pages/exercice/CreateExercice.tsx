import { useState } from "react";
import MarkdownComponent from "../../components/form/input/MarkdownComponent";
import AdminLayout from "../../components/layout/AdminLayout";

export default function CreateExercice() {
  const [situtaiton, setSituation] = useState<string>("");

  const updateSituation = (e: any) => {
    setSituation(e.target.value);
  };

  return (
    <AdminLayout>
      <div className="h-min-screen flex flex-col w-full space-y-4 mt-4 px-4">
        <div className="flex flex-col w-full">
          <h2 className="text-2xl font-semibold">Ajouter un exercice</h2>
        </div>
        <h3>Mise en situtaiton:</h3>
        <div
          className="grid overflow-auto h-96 grid-cols-2 divide-x-3 divide divide-gray-300 border border-dotted

 rounded-lg broder-gray-900"
        >
          <div>
            <textarea
              className="w-full  rounded-lg h-full p-1"
              value={situtaiton}
              onChange={updateSituation}
            ></textarea>
          </div>
          <div className="px-4 scrollable">
            <MarkdownComponent markdown={situtaiton} />
          </div>
        </div>
      </div>
    </AdminLayout>
  );
}
