import React from "react";

export default function ProductDetailLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
      <h1>product layout text</h1>
      {children}
    </>
  );
}
